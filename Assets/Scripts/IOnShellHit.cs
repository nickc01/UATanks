using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IOnShellHit
{
    //When this object is hit with a shell
    void OnShellHit(Shell shell);
}
