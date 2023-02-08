using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    public static partial class SfExport
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public async static Task<Stream> GetStreamAsync(this View view, ImageFileFormat format)
        {
            await Task.Delay(1000);
            return Stream.Null;
        }
    }
}
