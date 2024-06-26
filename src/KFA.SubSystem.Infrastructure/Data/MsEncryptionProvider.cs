﻿using Microsoft.EntityFrameworkCore.DataEncryption;

namespace KFA.SubSystem.Infrastructure.Data;
internal class MsEncryptionProvider : IEncryptionProvider
{
  public byte[]? Encrypt(byte[]? input) 
  {
     return input?.Reverse()?.ToArray();
  }
  public byte[]? Decrypt(byte[]? input)
  {
    return input?.Reverse()?.ToArray();
  }

  public MsEncryptionProvider() 
  {
  }
}
