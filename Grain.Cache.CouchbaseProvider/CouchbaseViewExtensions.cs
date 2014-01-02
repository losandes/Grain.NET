using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Grain.Extensions;

namespace Grain.Cache.CouchbaseProvider
{
    public static class CouchbaseViewExtensions
    {
        /// <summary>
        /// Transforms the resuts from Couchbase.GetView into a collection of T.  The main difference between this 
        /// and GetView&lt;T&gt; is that the Func used in this transformation allows the developer to define the 
        /// logic 
        /// logic 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <param name="factory"></param>
        /// <param name="invalidOpsHandler"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static IEnumerable<T> AndTransform<T>(this IView<IViewRow> view, Func<IViewRow, T> factory, bool omitNulls = true,
            Action<InvalidOperationException> invalidOpsHandler = null,
            Action<Exception> exceptionHandler = null) 
        {
            return view.AndTransform<IViewRow, T>(factory, omitNulls, invalidOpsHandler, exceptionHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RowType"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <param name="factory"></param>
        /// <param name="invalidOpsHandler"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        public static IEnumerable<T> AndTransform<RowType, T>(this IView<RowType> view, Func<RowType, T> factory, bool omitNulls = true,
            Action<InvalidOperationException> invalidOpsHandler = null,
            Action<Exception> exceptionHandler = null)
        {
            try
            {
                if(omitNulls)
                    return TransformView<RowType, T>(view, factory).Where(n => n != null);
                else return TransformView<RowType, T>(view, factory);
            }
            catch (InvalidOperationException ioe)
            {
                if (invalidOpsHandler != null)
                {
                    invalidOpsHandler(ioe);
                    return null;
                }
                else throw ioe;
            }
            catch (Exception e)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(e);
                    return null;
                }
                else throw e;
            }
        }

        /// <summary>
        /// Transforms the results from GetView, using the provided factory
        /// </summary>
        /// <typeparam name="RowType"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="view"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        private static IEnumerable<T> TransformView<RowType, T>(this IView<RowType> view, Func<RowType, T> factory) 
        { 
            if (view.IsEmpty())
                yield break;

            foreach (var row in view)
                yield return factory(row);        
        }
    }
}
