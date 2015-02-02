using System;

namespace FirebaseSharp
{
    public sealed class Firebase : IDisposable
    {
        private readonly Request _request;

        public Firebase(string rootUri, string authToken = null)
            : this(new Uri(rootUri), authToken)
        {            
        }

        public Firebase(Uri rootUri, string authToken = null)
        {
            if (rootUri == null)
            {
                throw new ArgumentNullException("rootUri");
            }

            _request = new Request(rootUri, authToken);
        }

        public Uri RootUri
        {
            get { return _request.RootUri; }
        }

        public string Post(string path, string payload)
        {
            return _request.Post(path, payload);
        }

        public string Put(string path, string payload)
        {
            return _request.Put(path, payload);
        }

        public string Patch(string path, string payload)
        {
            return _request.Patch(path, payload);
        }

        public void Delete(string path)
        {
            _request.Delete(path);
        }

        public string Get(string path)
        {
            return _request.GetSingle(path);
        }


        public Response GetStreaming(string path, 
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            return _request.GetStreaming(path, added, changed, removed);
        }

        public void Dispose()
        {
            using (_request) { }
        }
    }
}
