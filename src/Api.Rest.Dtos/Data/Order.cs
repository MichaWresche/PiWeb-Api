﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class is used for specifying the sort order of measurements.
	/// </summary>
	public class Order
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Order"/> class.
		/// </summary>
		public Order()
		{
			Direction = OrderDirection.Asc;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Order"/> class.
		/// </summary>
		/// <param name="attribute">The key of the attribute that is used for sorting</param>
		/// <param name="direction">The sort direction</param>
		/// <param name="entity">The entity that is used for sorting</param>
		public Order( ushort attribute, OrderDirection direction, Entity entity )
		{
			Attribute = attribute;
			Direction = direction;
			Entity = entity;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the entity that is used for sorting.
		/// </summary>
		[JsonProperty( "entity" )]
		public Entity Entity { get; set; }

		/// <summary>
		/// Gets or sets the key of the attribute that is used for sorting.
		/// </summary>
		[JsonProperty( "attribute" )]
		public ushort Attribute { get; set; }

		/// <summary>
		/// Gets or sets the sort direction.
		/// </summary>
		[JsonProperty( "direction" )]
		public OrderDirection Direction { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{Attribute} {Direction}";
		}

		#endregion
	}
}