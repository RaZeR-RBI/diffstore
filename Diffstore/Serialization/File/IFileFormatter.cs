using System;
using SharpFileSystem;

namespace Diffstore.Serialization.File
{
    public interface IFileFormatter<TIn, TOut> : IFormatter<TIn, TOut>, IDisposable
    {
         TIn BeginRead(IFileSystem fs, FileSystemPath path);
         TOut BeginWrite(IFileSystem fs, FileSystemPath path);
         void EndRead(TIn reader);
         void EndWrite(TOut writer);
    }
}