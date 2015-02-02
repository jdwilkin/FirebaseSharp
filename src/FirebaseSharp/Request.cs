using System;
using System.IO;
using System.Net;
using System.Text;

namespace FirebaseSharp
{
    internal sealed class Request : IDisposable
    {
        private readonly Uri _clientLocation;
        private readonly string _authToken;

        public Request(Uri rootUri, string authToken)
        {
            _clientLocation = rootUri;
            _authToken = authToken;
        }

        public Uri RootUri
        {
            get { return _clientLocation; }
        }

        public Response GetStreaming(string path, 
            ValueAddedEventHandler added = null,
            ValueChangedEventHandler changed = null,
            ValueRemovedEventHandler removed = null)
        {
            Uri uri = BuildPath(path);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BuildPath(path).ToString());
            request.Accept = "text/event-stream";
            request.Method = "GET";


            WebResponse responseObject = request.GetResponse();
            var responseStream = responseObject.GetResponseStream();


            return new Response(responseStream, added, changed, removed);
        }

        internal string GetSingle(string path)
        {


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BuildPath(path).ToString());
            request.Method = "GET";


            WebResponse responseObject = request.GetResponse();
            using (var responseStream = responseObject.GetResponseStream())
            using (var sr = new StreamReader(responseStream))
            {
                return sr.ReadToEnd();
            }
        }

        internal void Delete(string path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BuildPath(path).ToString());
            request.Method = "DELETE";


            using (WebResponse responseObject = request.GetResponse())
            {
                responseObject.Close();
            }
        }

        internal string Patch(string path, string payload)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BuildPath(path).ToString());
            request.Method = "PATCH";
            var data = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = data.Length;
            var stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            WebResponse responseObject = request.GetResponse();
            using (var responseStream = responseObject.GetResponseStream())
            using (var sr = new StreamReader(responseStream))
            {
                return sr.ReadToEnd();
            }
        }

        internal string Put(string path, string payload)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BuildPath(path).ToString());
            request.Method = "PUT";
            var data = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = data.Length;
            var stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            WebResponse responseObject = request.GetResponse();
            using (var responseStream = responseObject.GetResponseStream())
            using (var sr = new StreamReader(responseStream))
            {
                return sr.ReadToEnd();
            }
        }

        internal string Post(string path, string payload)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BuildPath(path).ToString());
            request.Method = "POST";
            var data = Encoding.UTF8.GetBytes(payload);
            request.ContentLength = data.Length;
            var stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            WebResponse responseObject = request.GetResponse();
            using (var responseStream = responseObject.GetResponseStream())
            using (var sr = new StreamReader(responseStream))
            {
                return sr.ReadToEnd();
            }
        }

        private Uri BuildPath(string path)
        {
            string uri = RootUri.AbsoluteUri + path;
            if (!string.IsNullOrEmpty(_authToken))
            {
                uri += string.Format("?auth={0}", _authToken);
            }

            return new Uri(uri);
        }


        public void Dispose()
        {
        }
    }
}
