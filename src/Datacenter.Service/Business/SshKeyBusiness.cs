using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Datacenter.Service.Business
{
    public class SshKeyBusiness : ISshKeyBusiness
    {
        private readonly ILogger<SshKeyBusiness> logger;
        private readonly ISshKeyRepository sshKeyRepository;
        private readonly Chilkat.SshKey sshKey = new Chilkat.SshKey();
        private readonly IMapper mapper;

        public SshKeyBusiness(ILogger<SshKeyBusiness> logger, ISshKeyRepository sshKeyRepository, IMapper mapper)
        {
            this.logger = logger;
            this.sshKeyRepository = sshKeyRepository;
            this.mapper = mapper;
        }

        public async Task<SshKeyResponse[]> ReadKeysAsync()
        {
            var dbKeys = await sshKeyRepository.ReadsAsync();
            return dbKeys.Select(k => mapper.Map<SshKeyResponse>(k)).ToArray();
        }

        public async Task<bool> DeleteKeyAsync(int id)
        {
            var dbKey = await sshKeyRepository.ReadAsync(id);

            if (dbKey != null)
            {
                dbKey.DeleteAt = DateTime.UtcNow;

                return (await sshKeyRepository.UpdateAsync(dbKey)) > 0;
            }

            return false;
        }

        public async Task<SshKeyDownloadResponse> DownloadAsync(int id, string type)
        {
            var dbKey = await sshKeyRepository.ReadAsync(id);

            if (dbKey != null)
            {
                if (type == "PEM")
                    return new SshKeyDownloadResponse() { Type = type, Name = $"{dbKey.Name}.pem", Content = dbKey.Pem };
                else
                    return new SshKeyDownloadResponse() { Type = type, Name = $"{dbKey.Name}.ppk", Content = dbKey.Pem };
            }

            return null;
        }


        public Task<bool> InsertKeyAsync(SshKeyCreateRequest request)
        {
            if (request.AutoGenerate)
            {
                return GenerateNewRsaKey(request);
            }
            else
            {
                return ImportRsaPublicKey(request);
            }
        }

        private async Task<bool> ImportRsaPublicKey(SshKeyCreateRequest request)
        {
            var importedKey = sshKey.FromOpenSshPublicKey(request.Public);
            var keyToInsert = new SshKey()
            {
                Fingerprint = sshKey.GenFingerprint(),
                Name = request.Name,
                Public = request.Public
            };

            if ((await sshKeyRepository.InsertAsync(keyToInsert)) > 0)
            {
                logger.LogInformation("Key insert.");

                return true;
            }

            return false;
        }

        private async Task<bool> GenerateNewRsaKey(SshKeyCreateRequest request)
        {
            var numBits = 2048;
            var exponent = 65537;
            var success = sshKey.GenerateRsaKey(numBits, exponent);
            string exportedPrivateKey, exportedPublicKey, exportedPpkKey;
            bool exportEncrypted;

            if (success != true)
            {
                logger.LogDebug("Bad params passed to RSA key generation method.");
                return false;
            }

            //  Export the RSA private key to OpenSSH, PuTTY, and XML and save.
            exportEncrypted = false;

            var fingerprint = sshKey.GenFingerprint();
            exportedPrivateKey = sshKey.ToOpenSshPrivateKey(exportEncrypted);
            exportedPpkKey = sshKey.ToPuttyPrivateKey(exportEncrypted);
            exportedPublicKey = sshKey.ToOpenSshPublicKey();

            var keyToInsert = new SshKey()
            {
                Fingerprint = fingerprint,
                Name = request.Name,
                Pem = exportedPrivateKey,
                Private = exportedPpkKey,
                Public = exportedPublicKey
            };

            if ((await sshKeyRepository.InsertAsync(keyToInsert)) > 0)
            {
                logger.LogInformation("Key insert.");

                return true;
            }

            return false;
        }
    }
}
