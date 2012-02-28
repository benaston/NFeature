NFeature
====

A simple feature configuration system (or feature toggle / flipper / whatever you want to call it.)

Feature configuration walls enable you to integrate your code earlier, which brings lots of goodness (such as helping to avoid branch merge problems.)

Example of use:

```C#


	if(Feature.MyCoolFeature.IsAvailable(manifest))
	{
		//do some cool stuff
	}


```

If NFeature helps you or your team develop great software please [let me know](mailto:ben@bj.ma "Ben's email address")! It will help motivate me to develop and improve NFeature.


How to use:
--------
**-1. Check the target framework of your application**

It *must* be ```.NET Framework 4``` (*not* the ```Client Profile``` version - or you might get strange compilation errors.)


**0. Get it**

```shell
	nuget install nfeature
```


**1. Define some features**

In your code:

```C#

	
	public enum Feature
	{
		MyFeature,
		MyOtherFeature,
		MyOtherOtherFeature,
	}
		

```

In your configuration: (see also footnote 1)

*NOTE: remember to replace ```Your.Feature.Type``` and ```Your.Feature.Type.Assembly``` in the text below.*

```XML
	...
  	<configSections>
		...
		<!-- note that if you are using your own tenant enum then you will have 
		to replace "Configuration.DefaultTenantEnum,  NFeature.Configuration"
		with your type -->
		<section name="features" type="NFeature.Configuration.FeatureConfigurationSection`2[[Your.Feature.Type, Your.Feature.Type.Assembly],[NFeature.Configuration.DefaultTenantEnum,  NFeature.Configuration]], NFeature.Configuration" />
		...
	</configSections>
	
	<features>
		<add name="MyFeature" state="Enabled" /> 
		<add name="MyOtherFeature" state="Previewable" /> <!-- will only be available to users who meet the feature-preview criteria -->
		<add name="MyOtherOtherFeature" state="Disabled" /> 
	</features>
	...
	
```

**2. Define the availability-checking function**


```C#


	//Here is a function that will only return 'true' if the feature is MyFeature.
	//Your function might be more elaborate. For example: feature availability might 
	//depend upon site load, user role or presence of a cookie.
	Func<FeatureSetting<Feature, DefaultTenantEnum>, EmptyArgs, bool> fn = (f, args) => f.Feature == Feature.MyFeature;

```

**3. Take care of feature manifest initialization**

*For a working example of this see the integration test named ```FeatureEnumExtensionsTests``` in the ```NFeature.Test.Slow``` project, within the main solution.*

```C#


	//NOTE: I suggest hiding this ugly initialization logic away in the IOC container configuration	
	var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature>();
	var availabilityChecker = new FeatureSettingAvailabilityChecker<Feature, EmptyArgs, DefaultTenantEnum>(fn); //from step 2
	var featureSettingService = new FeatureSettingService<Feature, DefaultTenantEnum, EmptyArgs>(availabilityChecker, featureSettingRepo);
	var manifestCreationStrategy = new NFeature.DefaultImplementations.ManifestCreationStrategyDefault<Feature, DefaultTenantEnum>(featureSettingRepo, featureSettingService); //we use the default for this example
	var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
	var featureManifest = featureManifestService.GetManifest();


```


**4. Configure feature dependencies (if there are any)**

```XML

	
    <features>
		<add name="MyFeature" dependencies="MyOtherFeature,MyOtherOtherFeature" />
	</features>

```


**5. Add code that is conditional on feature availability**
	
```C#


	if(Feature.MyFeature.IsAvailable(featureManifest)) //featureManifest ideally supplied via IOC container
	{
		//do some cool stuff
	}
	
```


**6. Optionally configure feature-specific settings using Json (neatly side-stepping the need for angle brackets)**

*Arbitrary JSON is supported through the use of ```dynamic``` objects.*

```XML

	
	<features>
		<add name="MyFeature" settings="{ mySetting:'mySettingValue', 
				   	                      myOtherSetting:['myOtherSettingValue','myOtherOtherSettingValue'] }" />
	</features>

```

**7. Optionally specify dates for feature availability**

```XML

	
    <features>
		<add name="MyFeature" startDtg="23/03/2012:00:00:00" /> <!-- available from 23rd March 2012 forever -->
		<add name="MyOtherFeature" startDtg="23/03/2012:00:00:00" endDtg="24/03/2012:00:00:00" /> <!-- available from 23rd March 2012 until the 24th -->
		<add name="MyOtherOtherFeature" endDtg="24/03/2012:00:00:00" /> <!-- available until 24th March 2012 -->
	</features>

```

**8. At some future date... optionally mark your feature as ```Established``` to indicate that it is now integral to your application and cannot be turned off (see footnote 2)**

*Any availability checks for ```Established``` features will throw an exception, forcing their removal (as is good practice.)*

```XML

	
	<features>
		<add name="MyFeature" state="Established" />
	</features>

```

**9. ...**

**10. Profit!**


How to build and/or run the tests:
--------

1. Run `/build/build.bat`
1. Type in the desired option
1. Hit return



**Footnote 1:**
Please note that the logic to determine whether a feature is available is specified in the ```IFeatureManifestCreationStrategy``` instance you inject into the ```FeatureManifestService``` and (optionally, depending on your implementation of the aforementioned strategy) by the availability-checking function you inject into the ```FeatureSettingAvailabilityChecker```. 

Two concrete implementations of ```IFeatureManifestCreationStrategy``` are provided of-the-box: ```ManifestCreationStrategyDefault``` and ```ManifestCreationStrategyConsideringStateCookieTenantAndTime```. A single default availability checker function is provided out of the box ```DefaultFunctions.AvailabilityCheckFunction```, which may be used when the feature state, tenant, feature visibility mode and system time are known.

**Footnote 2:**
Marking a feature as established changes the behavior of the feature in the following way:

 - all dependencies must be established
 - checking the feature's availability will throw an exception (because it is now always available by definition)


**NOTE: this is pre-release quality software. There will be bugs/inaccuracies in the documentation. If you find an issue, please help me by adding an issue here on GitHub.**


License & Copyright
--------

This software is released under the GNU Lesser GPL. It is Copyright 2011, Ben Aston. I may be contacted at ben@bj.ma.