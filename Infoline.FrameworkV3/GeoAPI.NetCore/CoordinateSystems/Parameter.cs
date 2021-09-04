// Copyright 2005, 2006 - Morten Nielsen (www.iter.dk)
//
// This file is part of SharpMap.
// SharpMap is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;

namespace GeoAPI.CoordinateSystems
{
    /// <summary>
    /// Ýsimlendirilmiþ bir parametre deðeri..
    /// </summary>
#if !PCL && !NET_CORE
    [Serializable]
#endif
    public class Parameter
    {
        /// <summary>
        /// Parametrenin bir örneðini oluþturur
        /// </summary>
        /// <remarks>Birimler her zaman metre veya derece olur.</remarks>
        /// <param name="name">Parametrenin adý</param>
        /// <param name="value">Parametrenin deðeri</param>
        public Parameter(string name, double value)
		{
			_Name = name;
			_Value = value;
		}

	    private string _Name;

		/// <summary>
		/// Parametre adý
		/// </summary>
        public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		private double _Value;

		/// <summary>
		/// Parametre deðeri
		/// </summary>
        public double Value
		{
			get { return _Value; }
			set { _Value = value; }
		}
	}
}
