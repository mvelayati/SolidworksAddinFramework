﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SolidworksAddinFramework
{
    public static class DisposableExtensions
    {
        public static IDisposable ToCompositeDisposable(this IEnumerable<IDisposable> d)
        {
            return new CompositeDisposable(d);
        }
    }
}