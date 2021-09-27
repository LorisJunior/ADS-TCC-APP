using System;

namespace TCCApp.Serviços
{
    public interface IOAuth2Service
    {
        event EventHandler<string> OnSuccess;
        event EventHandler<string> OnError;
        event EventHandler OnCancel;

        void Authenticate(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl);
    }
}
