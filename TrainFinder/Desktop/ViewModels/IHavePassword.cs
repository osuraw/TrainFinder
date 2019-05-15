using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.ViewModels
{
    public interface IHavePassword
    {
        SecureString SecureString { get; }
    }
}
