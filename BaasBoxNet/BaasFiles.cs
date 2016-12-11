using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaasBoxNet
{
    class BaasFiles : IBaasFiles
    {
        private BaasBox _box;

        public BaasFiles(BaasBox box)
        {
            _box = box;
        }

        public async Task<byte[]> GetFile(string fileId)
        {
            return await _box.RestService.GetFile(fileId, CancellationToken.None);
        }
    }
}
