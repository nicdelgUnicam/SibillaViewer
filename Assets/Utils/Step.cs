using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Represents a step of the simulation
    /// </summary>
    public class Step
    {
        public float Time { get; private set; }
        public Vector3 Position { get; private set; }


        private Step(float time, float x, float y, float z) : this(time, new Vector3(x, y, z)) { }

        private Step(float time, Vector3 position)
        {
            Time = time;
            Position = position;
        }
        
        
        public static Step FromLine(string[] line, int[] indices)
        {
            // WebGL doesn't detect the correct decimal separator so it has to be set
            var ci = System.Globalization.CultureInfo.InstalledUICulture;
            var ni = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();
            ni.NumberDecimalSeparator = ".";
            
            var time = float.Parse(line[indices[0]], ni);
            var x = float.Parse(line[indices[1]], ni);
            var y = float.Parse(line[indices[2]], ni);
            //var z = line.Length > 3 ? float.Parse(line[indices[3]], ni) : 0.0f;
            return new Step(time, x, y, 0.0f);
        }
    }
}