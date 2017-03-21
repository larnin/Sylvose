using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class G
{
    private static volatile G instance;

    public static G Sys
    {
        get
        {
            if (G.instance == null)
                G.instance = new G();
            return G.instance;
        }
    }

}
