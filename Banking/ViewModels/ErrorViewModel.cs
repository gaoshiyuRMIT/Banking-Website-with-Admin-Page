using System;

namespace Banking.ViewModels
{
    public class ErrorViewModel
    {
        public int Code {get;set;}
        public string Name {
            get {
                if (Code == 400)
                    return "Bad Request";
                if (Code == 401)
                    return "Unauthorized";
                if (Code == 403)
                    return "Forbidden";
                if (Code == 404)
                    return "Not Found";
                if (Code == 405)
                    return "Method Not Allowed";
                if (Code == 408)
                    return "Request Timeout";
                if (Code == 410)
                    return "Gone";
                if (Code == 500)
                    return "Internal Server Error";
                return null;
            }
        }
        public string Message {
            get {
                if (Code == 400)
                    return "The server cannot understand the request, possibly due to invalid request syntax.";
                if (Code == 401)
                    return "You are not authorized to access this resource.";
                if (Code == 403)
                    return "You need authentication to access this recourse.";
                if (Code == 404)
                    return "The resource you requested cannot be found.";
                if (Code == 405)
                    return "This method is allowed on this resource.";
                if (Code == 408)
                    return "The server does not respond with time limits";
                if (Code == 410)
                    return "This resource is no longer available";
                if (Code == 500)
                    return "It's not you. It's us. To understand the problem, please contact System Administrator.";
                return null;
            }
        }
    }
}
