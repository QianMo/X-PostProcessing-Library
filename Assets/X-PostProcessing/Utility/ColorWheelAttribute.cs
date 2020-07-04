using System;

namespace XPostProcessing
{
    /// <summary>
    /// Use this attribute to draw a ColorWheel in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ColorWheelAttribute : Attribute
    {
        /// <summary>
        /// ColorWheel modes. These are used to compute and display pre-filtered ColorWheel vales in
        /// the inspector.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Don't display pre-filtered values.
            /// </summary>
            None,

            /// <summary>
            /// Display pre-filtered lift values.
            /// </summary>
            Lift,

            /// <summary>
            /// Display pre-filtered gamma values.
            /// </summary>
            Gamma,

            /// <summary>
            /// Display pre-filtered grain values.
            /// </summary>
            Gain,


            Contrast
        }

        /// <summary>
        /// The mode used to display pre-filtered values in the inspector.
        /// </summary>
        public readonly Mode mode;

        /// <summary>
        /// Creates a new attribute.
        /// </summary>
        /// <param name="mode">A mode used to display pre-filtered values in the inspector</param>
        public ColorWheelAttribute(Mode mode)
        {
            this.mode = mode;
        }
    }


}