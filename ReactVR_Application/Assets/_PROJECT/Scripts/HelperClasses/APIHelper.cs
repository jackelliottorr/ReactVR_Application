using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._PROJECT.Scripts.HelperClasses
{
    public class APIHelper
    {
        const string _baseUriString = "http://reactvrapi.azurewebsites.net/api/";
        private Uri _baseUri = null;

        public Uri GetBaseUri()
        {
            if (_baseUri == null)
            {
                _baseUri = new Uri(_baseUriString);
                return _baseUri;
            }
            else
            {
                return _baseUri;
            }
        }
    }
}
