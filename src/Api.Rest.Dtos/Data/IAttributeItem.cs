﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	/// <summary>
	/// Interface für Objekte, die eine Liste von Attributen zur Verfügung stellen.
	/// </summary>
	public interface IAttributeItem
	{
		#region properties

		/// <summary>
		/// Gibt alle Attribute als Feld zurück.
		/// </summary>
		Attribute[] Attributes { get; set; }

		#endregion
	}
}