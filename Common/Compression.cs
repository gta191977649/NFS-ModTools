using System;
using System.IO;
//using CompLib;
using System.IO.Compression;


namespace Common
{
    public static class BlobDecompressor
    {
        public static Span<byte> Decompress(ReadOnlySpan<byte> input)
        {
            using MemoryStream inputMemoryStream = new MemoryStream(input.ToArray());
            using GZipStream gzipStream = new GZipStream(inputMemoryStream, CompressionMode.Decompress);
            using MemoryStream outputMemoryStream = new MemoryStream();
            gzipStream.CopyTo(outputMemoryStream);
            return new Span<byte>(outputMemoryStream.ToArray());
        }
    }
    
    public static class CipDecompressor
    {
        public static void Decompress(Stream src, Stream dst, long srcSize, out long decompressedSize)
        {
            using GZipStream gzipStream = new GZipStream(src, CompressionMode.Decompress);
            gzipStream.CopyTo(dst);
            decompressedSize = dst.Length;
        }
    }
    
    /// <summary>
    /// Wrapper around CompLib API 
    /// </summary>
    public static class Compression
    {
        public static Span<byte> Decompress(ReadOnlySpan<byte> input)
        {
            return BlobDecompressor.Decompress(input);
        }

        public static long DecompressCip(Stream src, Stream dst, long srcSize)
        {
            CipDecompressor.Decompress(src, dst, srcSize, out var decompressedSize);

            return decompressedSize;
        }
    }
}