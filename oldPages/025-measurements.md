---
category: dataservice
subCategory: measurements
title: Data Service
subTitle: Measurements and Measured Values
isSubPage: true
permalink: /dataservice/measurements/
sections:
  general: General Information
  endpoint: REST API Endpoints
  sdk: .NET SDK Methods
---

## {{ page.sections['general'] }}

Measurements do always belong to a single inspection plan part. Depending on the purpose, the measured values are included within a measurement (```DataMeasurement```) or not (```SimpleMeasurement```). Each measurement consists of the following properties:

### SimpleMeasurement

Name         | Type        | Description
-------------|-------------|--------------
uuid         | Guid        | Identifies the measurement uniquely.
partUuid     | Guid        | The uuid of the part the measurement belongs to.
attributes   | Attribute[] | A set of attributes which specifies this measurement.
lastModified | DateTime    | Contains the date and time of the last update applied to this measurement.

### DataMeasurement : SimpleMeasurement

Name            | Type                 | Description
----------------|----------------------|--------------
characteristics | DataCharacteristic[] | An array of the characteristics which has been measured within the measurement. Each characteristic within this array consits of the uuid it is identified by and an array of attributes which include at least the measured value attribute.

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Measurements can be fetched, created, updated and deleted via the following endpoints.

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|------|-----|-------
/parts/measurements | Returns all measurements without measured values | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | Updates the committed measurements | Deletes all measurements.
/parts/:partPath/measurements | Returns the measurements without measured values which belong to the part specified by *:partPath*  | *--* | *--* | Deletes the measurements which belong to the part specified by *:partPath*
/parts/(:partUuids)/measurements | Returns the measurements without measured values which belong to the parts specified by *:partUuids*  | *--* | *--* | *--*
/parts/measurements/{:uuidList} | Returns the measurements without measured values which belong to the parts of which the uuid is within the *:uuidList* | *--* | *--* |  Deletes all measurements which belong to the parts of which the uuid is within the *:uuidList*
/parts/measurements/values | Returns all measurements including measured data | Creates the committed measurements which is/are transfered in the body of the request. These measurements contain measured values. | Updates the committed measurements including measured values. |*--*
/parts/:partPath/measurements/values | Returns the measurements including measured values which belong to the part specified by *:partPath* | *--* | *--* | *--*
/parts/(:partUuids)/measurements/values | Returns the measurements including measured values which belong to the parts specified by *:partUuids* | *--* | *--* | *--*
/parts/measurements/{:uuidList}/values | Returns the measurements including measured values which belong to the parts of which the uuid is within the *:uuidList* | *--* | *--* | *--*

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

### Get measurements

Measurements can be fetched in several ways and for several purposes:

* fetch all measurements - with or without values
* fetch measurements for a particular part by its path - with or without values
* fetch measurements for particular parts by their uuids - with or without values
* fetch measurements by their uuids - with or without values

Each request can be restricted by the respective filter values as described in the [URL-Parameter section]({{site.baseurl }}/general/restapi/#{{ page.subCategory }})

{% assign exampleCaption="Fetch all measurements and values for the part e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}
{% assign comment="The request is further restricted to only measurements newer than 01.01.2015" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/parts/(e42c5327-6258-4c4c-b3e9-6d22c30938b2)/measurements/values?filter=searchCondition:4>[2015-01-01T00:00:00Z] HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
{
  ...
   "data":
   [
     {
       "characteristics":
       {
           "360f55e5-77c3-49f9-9a5e-80d0a9040e2d":
           {
               "1": "0.24966522"
           },
           "b5c98235-c75c-41a4-aced-2a38c70a3866":
           {
               "1": "0.4457339"
           },
           "85bbb406-810e-4062-8a9f-c7b636cb61bd":
           {
               "1": "0.24981162"
           }
       },
       "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
       "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
       "lastModified": "2015-03-09T09:19:38.653Z",
       "attributes":
       {
           "4": "2015-03-09T19:12:00Z",
           "6": "3",
           "7": "0"
       }
      },
      ...
}
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

### Add measurements

Measurements can be created with or without measured values. To create a measurement, it is necessary to transfer the measurement object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 

{% assign exampleCaption="Create a measurement without values" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "3",
        "7": "0"
      }     
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 201,
       "statusDescription": "Created"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

{% assign exampleCaption="Create a measurement with measured values" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements/values HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "3",
        "7": "0"
      },
      "characteristics":
      {
         "360f55e5-77c3-49f9-9a5e-80d0a9040e2d":
         {
             "1": "0.24966522"
         },
         "b5c98235-c75c-41a4-aced-2a38c70a3866":
         {
             "1": "0.4457339"
         },
         "85bbb406-810e-4062-8a9f-c7b636cb61bd":
         {
             "1": "0.24981162"
         }
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 201,
       "statusDescription": "Created"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

### Update measurements

Updating a measurement does always affect the whole measurement. This means that the whole measurement, including attributes and values, needs to be transfered within the body of the request and is deleted and recreated again on server side.

{% assign exampleCaption="Update a measurement - add and change an attribute" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "2",
        "7": "0",
        "8": "1"
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

### Delete measurements

There are three possibilities to delete measurements:

* Delete all measurements
* Delete measurements from a particular part by its path
* Delete measurements by their uuids

{% assign exampleCaption="Delete the measurement with the uuid 4b59cac7-9ecd-403c-aa26-56dd25892421" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements/(4b59cac7-9ecd-403c-aa26-56dd25892421) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

{% comment %}------------------------------------------------------------------------------------------------{% endcomment %}

## {{ page.sections['sdk'] }}

### Get Measurements

#### Without Values

{% assign caption="GetMeasurementsForPart" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches measurements for the ```part```." %}
{% capture parameterTable %}

 Name          | Type                              | Description
---------------|-----------------------------------|--------------------------------------------------
part           | ```PathInformation```             | The path of the part all measurements shall be fetched for.
filter         | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token          | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<SimpleMeasurement[]>" %}

{% assign exampleCaption="Get all measurements for the part 'metal part'" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurements = await client.GetMeasurementsForPart( PathHelper.String2PartPathInformation( "/metal part" ) );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="GetMeasurementsForParts" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches measurements for all parts that uuid is within ```parts```" %}
{% capture parameterTable %}

 Name          | Type                              | Description
---------------|-----------------------------------|--------------------------------------------------
parts          | ```Guid[]```                       | List of uuids of the parts the measurements shall be fetched for.
filter         | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token          | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<SimpleMeasurement[]>" %}

{% assign exampleCaption="Get all measurements for the part with the uuid '1429c5e2-599c-4d3e-b724-4e00ecb0caa7'" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurements = await client.GetMeasurementsForParts( new[]{ new Guid( "1429c5e2-599c-4d3e-b724-4e00ecb0caa7" ) } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="GetMeasurementsByUuid" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches all measurements that uuids are within ```measurementUuids```" %}
{% capture parameterTable %}

 Name            | Type                              | Description
-----------------|-----------------------------------|--------------------------------------------------
measurementUuids | ```Guid[]```                      | List of uuids of the measurements that shall be fetched.
filter           | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token            | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<SimpleMeasurement[]>" %}

{% assign exampleCaption="Get the measurements with the uuid '5b59cac7-9ecd-403c-aa26-56dd25892421'" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurements = client.GetMeasurementsByUuid( new[]{ new Guid( "5b59cac7-9ecd-403c-aa26-56dd25892421" ) } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

#### With Values

{% assign caption="GetMeasuredValuesForPart" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches measurements including the measured values for the ```part```." %}
{% capture parameterTable %}

 Name          | Type                              | Description
---------------|-----------------------------------|--------------------------------------------------
part           | ```PathInformation```             | The path of the part all measurements shall be fetched for.
filter         | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token          | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<DataMeasurement[]>" %}

{% assign exampleCaption="Get all measurements including the measured values for the part 'metal part'" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurements = await client.GetMeasuredValuesForPart( PathHelper.String2PartPathInformation( "/metal part" ) );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="GetMeasuredValuesForParts" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches measurements including measured values for all parts that uuid is within ```parts```" %}
{% capture parameterTable %}

 Name          | Type                              | Description
---------------|-----------------------------------|--------------------------------------------------
parts          | ```Guid[]```                      | List of uuids of the parts the measurements shall be fetched for.
filter         | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token          | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<DataMeasurement[]>" %}

{% assign exampleCaption="Get all measurements including measured values for the part with the uuid '1429c5e2-599c-4d3e-b724-4e00ecb0caa7'" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurements = await client.GetMeasuredValuesForParts( new[]{ new Guid( "1429c5e2-599c-4d3e-b724-4e00ecb0caa7" ) } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="GetMeasuredValuesForMeasurements" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches all measurements including measured values for measurements of which the uuid is within ```measurementUuids```" %}
{% capture parameterTable %}

 Name            | Type                              | Description
-----------------|-----------------------------------|--------------------------------------------------
measurementUuids | ```Guid[]```                      | List of uuids of the measurements that shall be fetched.
filter           | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token            | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<DataMeasurement[]>" %}

{% assign exampleCaption="Get the measurements including measured values for the measurement with the uuid '5b59cac7-9ecd-403c-aa26-56dd25892421'" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurements = client.GetMeasuredValuesForMeasurements( new[]{ new Guid( "5b59cac7-9ecd-403c-aa26-56dd25892421" ) } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Create Measurements

{% assign caption="CreateMeasurements" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Creates a measurement without measured values." %}
{% capture parameterTable %}

 Name            | Type                              | Description
-----------------|-----------------------------------|--------------------------------------------------
measurements     | ```SimpleMeasurement[]```         | List of the measurements that shall be created.
token            | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task" %}

{% assign exampleCaption="Create a measurement" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurement = new SimpleMeasurement
                  { 
                    Uuid= Guid.NewGuid(),
                    PartUuid = new Guid( "e42c5327-6258-4c4c-b3e9-6d22c30938b2" ),
                    Attributes = new {}
                    [
                      new Attribute( 4, new DateTime( 2015,3,9,19,12 ) ),
                      new Attribute ( 6, "3" ),
                      new Attribute ( 7, "0" )
                    ]
                  }
client.CreateMeasurement( new []{ measurement } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="CreateMeasuredValues" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Creates a measurement including measured values." %}
{% capture parameterTable %}

 Name            | Type                              | Description
-----------------|-----------------------------------|--------------------------------------------------
measurements     | ```DataMeasurement[]```           | List of the measurements, including values, to be created.
token            | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task" %}

{% assign exampleCaption="Create a measurement including measured values" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var valueMeasurement = new SimpleMeasurement
                  { 
                    Uuid= Guid.NewGuid(),
                    PartUuid = new Guid( "e42c5327-6258-4c4c-b3e9-6d22c30938b2" ),
                    Attributes = new {}
                    [
                      new Attribute( 4, new DateTime( 2015,3,9,19,12 ) ),
                      new Attribute ( 6, "3" ),
                      new Attribute ( 7, "0" )
                    ],
                    Characteristics = new {}
                    [
                      new DataCharacteristic ( "0.24966522" ),
                      new DataCharacteristic ( "0.4457339" ),
                      new DataCharacteristic ( "0.24981162" )
                    ]
                  }
client.CreateMeasuredValues( new []{ valueMeasurement } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}


### Update Measurements

{% assign caption="UpdateMeasurements" %}
{% assign icon=site.images['function-update'] %}
{% assign description="Updates a measurement without measured values." %}
{% capture parameterTable %}

 Name            | Type                              | Description
-----------------|-----------------------------------|--------------------------------------------------
measurements     | ```SimpleMeasurement[]```         | List of to be updated measurements.
token            | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task" %}

{% assign exampleCaption="Update a measurement - add an attribute" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measurement = new SimpleMeasurement
                  { 
                    Uuid= Guid.NewGuid(),
                    PartUuid = new Guid( "e42c5327-6258-4c4c-b3e9-6d22c30938b2" ),
                    Attributes = new {}
                    [
                      new Attribute( 4, new DateTime( 2015,3,9,19,12 ) ),
                      new Attribute ( 6, "3" ),
                      new Attribute ( 7, "0" ),
                      new Attribute ( 8, "1" )
                    ]
                  }
client.UpdateMeasurement( new []{ measurement } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="UpdateMeasuredValues" %}
{% assign icon=site.images['function-update'] %}
{% assign description="Updates a measurement including measured values." %}
{% capture parameterTable %}

 Name            | Type                              | Description
-----------------|-----------------------------------|--------------------------------------------------
measurements     | ```DataMeasurement[]```           | List of the measurements, including values, to be updated.
token            | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task" %}

{% assign exampleCaption="Update a measurement including measured values - add another measured value" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var valueMeasurement = new SimpleMeasurement
                  { 
                    Uuid= Guid.NewGuid(),
                    PartUuid = new Guid( "e42c5327-6258-4c4c-b3e9-6d22c30938b2" ),
                    Attributes = new {}
                    [
                      new Attribute( 4, new DateTime( 2015,3,9,19,12 ) ),
                      new Attribute ( 6, "3" ),
                      new Attribute ( 7, "0" )
                    ],
                    Characteristics = new {}
                    [
                      new DataCharacteristic ( "0.24966522" ),
                      new DataCharacteristic ( "0.4457339" ),
                      new DataCharacteristic ( "0.24981162" ),
                      new DataCharacteristic ( "0.24467985" )
                    ]
                  }
clientUpdateMeasuredValues( new []{ valueMeasurement } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Delete Measurements

{% assign caption="DeleteMeasurements" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes measurements which belong to the ```part```." %}
{% capture parameterTable %}

 Name          | Type                              | Description
---------------|-----------------------------------|--------------------------------------------------
part           | ```PathInformation```             | The path of the part all measurements shall be deleted for.
filter         | ```MeasurementFilterAttributes``` | Parameter is optional and may restrict the query.
token          | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task" %}

{% assign exampleCaption="Delete all measurements from the part 'metal part' which are older than 01.01.2015" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var measSearchCondition = new MeasurementSearchAnd
  {
  	Conditions = new MeasurementSearchAttributeCondition 
  		{
  		  Attribute = 4, 
  		  Operation = Operation.LessThan, 
  		  Value = XmlConvert.ToString( new DateTime( 2015, 1, 1, 0, 0, 0, DateTimeKind.Utc ),
  		          System.Xml.XmlDateTimeSerializationMode.RoundtripKind ), 
  		  Entity = MeasurementSearchEntity.Measurement
  		}
  };
			
var filter = new MeasurementFilterAttributes()
  { 
    SearchCondition = measSearchCondition
  };
client.DeleteMeasurements( PathHelper.String2PartPathInformation( "/metal part" ), filter );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="DeleteMeasurements" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes measurements of which the uuid is within ```measurements```." %}
{% capture parameterTable %}

 Name          | Type                              | Description
---------------|-----------------------------------|--------------------------------------------------
measurements   | ```Guid[]```                      | The uuids of the measurements to be deleted.
token          | ```CancellationToken```           | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task" %}

{% assign exampleCaption="Delete all measurements with the uuid 5b59cac7-9ecd-403c-aa26-56dd25892421" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.DeleteMeasurements( new Guid{}[ new Guid( "5b59cac7-9ecd-403c-aa26-56dd25892421" )  ] );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}
