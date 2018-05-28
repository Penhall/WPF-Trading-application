using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.SocketConnection
{
    public class CompressUtility
    {
        [DllImport("JavaLZO.dll")]
        private static extern int lzo1x_1_compress(byte[] src, int src_len,
            byte[] dst, ref IntPtr dst_len,
            byte[] wrkmem);
        [DllImport("JavaLZO.dll")]
        private static extern int lzo1x_decompress(byte[] src, int src_len,
            byte[] dst, ref IntPtr dst_len,
            byte[] wrkmem);
        [DllImport("JavaLZO.dll")]
        private static extern int lzoinit();

        public static int LZOInit()
        {
            try
            {
                return lzoinit();
            }
            catch (Exception lException)
            {
                throw lException;
            }
        }
        public static int Compress(byte[] pSource, int pSourceLength, byte[] pDestination, byte[] pWorkMemory)
        {
            int lResult;
            IntPtr lCompressedLength;
            lCompressedLength = new System.IntPtr(1);
            lResult = lzo1x_1_compress(pSource, pSourceLength, pDestination, ref lCompressedLength, pWorkMemory);
            if (lResult != 0) throw new Exception("Error " + lResult + " during decompression.");
            return lCompressedLength.ToInt32();
        }
        public static int Compress(String pSource, byte[] pDestination, byte[] pWorkMemory)
        {
            byte[] lSource;
            int lCount;
            lSource = new byte[pSource.Length];
            for (lCount = 0; lCount < pSource.Length; lCount++)
            {
                lSource[lCount] = (byte)pSource[lCount];
            }
            return Compress(lSource, lSource.Length, pDestination, pWorkMemory);
        }
        public static int DeCompress(byte[] pSource, int pSourceLength, byte[] pDestination, byte[] pWorkMemory)
        {
            try
            {

                int lResult;
                IntPtr lDeCompressedLength;
                lDeCompressedLength = new System.IntPtr();
                ////				System.Console.WriteLine("Inside DeCompress Started");
                lResult = lzo1x_decompress(pSource, pSourceLength, pDestination, ref lDeCompressedLength, pWorkMemory);
                ////				System.Console.WriteLine("Inside DeCompress Finished");
                if (lResult != 0) throw new Exception("Error " + lResult + " during decompression.");
                return lDeCompressedLength.ToInt32();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("DeCompress Exception  : " + ex.Message);
                return 0;
            }

        }
    }
}
