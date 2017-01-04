//===============================================================================
// Microsoft Patterns & Practices Enterprise Library
// Excerpt from the Exception Handling Logging Block
//===============================================================================

using System;

namespace Microsoft.ApplicationBlocks.ExceptionHandling
{
    /// <summary>
    /// Defines the contract for an ExceptionHandler.  An ExceptionHandler contains specific handling
    /// logic (i.e. logging the exception, replacing the exception, etc.) that is executed in a chain of multiple
    /// ExceptionHandlers.  A chain of one or more ExceptionHandlers is executed based on the exception type being 
    /// handled, as well as the <see cref="ExceptionPolicy"/>.  <seealso cref="ExceptionPolicy.HandleException"/>
    /// </summary>    
    public interface IExceptionHandler
    {
        /// <summary>
        /// <para>When implemented by a class, handles an <see cref="Exception"/>.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>        
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        void HandleException(Exception exception, Guid handlingInstanceId);
    }
}