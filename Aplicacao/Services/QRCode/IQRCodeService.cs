﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Services.QRCode
{
    public interface IQRCodeService
    {
        Task<byte[]> GerarQrCodeAsync(string qrCodeString);
    }
}
