using System;
using System.IO;
using System.Text;
using log4net;

namespace Org.Owasp.CsrfGuard.ResponseFilters
{
    /// <summary>
    /// Summary description for ResponseFilterBase.
    /// </summary>
    public abstract class ResponseFilterBase : Stream
    {
        protected String _CSRFTokenName;
        protected String _CSRFSesssionToken;
        protected Stream _responseStream;
        protected long _position;
        protected StringBuilder _responseHtml;
        protected static readonly ILog _log = LogManager.GetLogger("CSRFGuard");

        protected ResponseFilterBase(Stream inputStream, String tokenName, String token)
        {
            _responseStream = inputStream;
            _responseHtml = new StringBuilder(); // html is appended gradually by the Write() method
            _CSRFSesssionToken = token;
            _CSRFTokenName = tokenName;
        }

        protected ResponseFilterBase()
        {
            throw new NotImplementedException();
        }

        #region Test harness accessors

        public StringBuilder GetResponseHtml
        {
            get { return _responseHtml; }
        }

        #endregion

        #region Filter overrides

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Close()
        {
            _responseStream.Close();
        }

        public override void Flush()
        {
            _responseStream.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _responseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _responseStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _responseStream.Read(buffer, offset, count);
        }

        #endregion

        #region Do the rewriting

        // This is the opportunity to rewrite the HTML before sending back to the browser
        public override void Write(byte[] buffer, int offset, int count)
        {
            // implement in subclasses to do the actual filtering work
        }

        #endregion
    }
}