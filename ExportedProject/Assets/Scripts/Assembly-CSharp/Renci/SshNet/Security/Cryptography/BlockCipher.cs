using System;
using Renci.SshNet.Security.Cryptography.Ciphers;

namespace Renci.SshNet.Security.Cryptography
{
	public abstract class BlockCipher : SymmetricCipher
	{
		private readonly CipherMode _mode;

		private readonly CipherPadding _padding;

		private readonly byte _blockSize;

		public override byte MinimumSize
		{
			get
			{
				return BlockSize;
			}
		}

		public byte BlockSize
		{
			get
			{
				return _blockSize;
			}
		}

		protected BlockCipher(byte[] key, byte blockSize, CipherMode mode, CipherPadding padding)
			: base(key)
		{
			_blockSize = blockSize;
			_mode = mode;
			_padding = padding;
			if (_mode != null)
			{
				_mode.Init(this);
			}
		}

		public override byte[] Encrypt(byte[] data)
		{
			if (data.Length % _blockSize > 0)
			{
				if (_padding == null)
				{
					throw new ArgumentException("data");
				}
				data = _padding.Pad(_blockSize, data);
			}
			byte[] array = new byte[data.Length];
			int num = 0;
			for (int i = 0; i < data.Length / _blockSize; i++)
			{
				num = ((_mode != null) ? (num + _mode.EncryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)) : (num + EncryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)));
			}
			if (num < data.Length)
			{
				throw new InvalidOperationException("Encryption error.");
			}
			return array;
		}

		public override byte[] Decrypt(byte[] data)
		{
			if (data.Length % _blockSize > 0)
			{
				if (_padding == null)
				{
					throw new ArgumentException("data");
				}
				data = _padding.Pad(_blockSize, data);
			}
			byte[] array = new byte[data.Length];
			int num = 0;
			for (int i = 0; i < data.Length / _blockSize; i++)
			{
				num = ((_mode != null) ? (num + _mode.DecryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)) : (num + DecryptBlock(data, i * _blockSize, _blockSize, array, i * _blockSize)));
			}
			if (num < data.Length)
			{
				throw new InvalidOperationException("Encryption error.");
			}
			return array;
		}
	}
}
