using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Util
{
    public static CancellationTokenSource CancelAndNew(this CancellationTokenSource cts)
    {
        cts.Cancel();
        cts.Dispose();
        cts = new();
        return cts;
    }
}
