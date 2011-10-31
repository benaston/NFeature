NFeature
====

A simple feature configuration system. 

Feature configuration walls enable you to integrate your code earlier, which brings lots of goodness (such as helping to avoid branch merge problems.)

How to use:
--------
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


```XML

	
    <features>
		<add name="MyFeature" state="Enabled" /> 
		<add name="MyOtherFeature" state="Previewable" /> <!-- will only be available to users who meet the feature-preview criteria -->
		<add name="MyOtherOtherFeature" state="Disabled" /> 
	</features>
	
```

**2. Define the availability-checking function**


```C#


	//Here is a function that will only return 'true' if the feature is TestFeatureA
	//Your function might be more elaborate. For example: feature availability might 
	//depend upon site load, user role or presence of a cookie.
	Func<FeatureSetting<Feature>, EmptyArgs, bool> fn = (f, args) => f == Feature.TestFeatureA; 

```

**3. Take care of feature manifest initialization**

For a working example of this see the integration test named ```FeatureEnumExtensionsTests``` in the ```NFeature.Test.Slow``` project, within the main solution.

```C#


	//NOTE: I suggest hiding this ugly initialization logic away in the IOC container configuration	
	var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature>();
	var availabilityChecker = new FeatureSettingAvailabilityChecker<Feature>(fn); //from step 2      
	var featureSettingService = new FeatureSettingService<Feature, EmptyArgs>(availabilityChecker, featureSettingRepo);
	var manifestCreationStrategy = new ManifestCreationStrategyDefault(featureSettingRepo, featureSettingService); //we use the default for this example
	var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
	var featureManifest = featureManifestService.GetManifest();	


```

**4. Add code that is conditional on feature availability**
	
```C#


	if(Feature.MyFeature.IsAvailable(featureManifest)) //featureManifest ideally supplied via IOC container
	{
		//do some cool stuff
	}
	
```

**5. Configure feature dependencies**

```XML

	
    <features>
		<add name="MyFeature" dependencies="MyOtherFeature,MyOtherOtherFeature" />
	</features>

```

**6. Optionally configure feature settings using Json (neatly side-stepping the Microsoft XML configuration functionality)**
	
```XML

	
	<features>
		<add name="MyFeature" settings="{ mySetting:'mySettingValue', 
				   	                      myOtherSetting:'myOtherSettingValue' }" />
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


IOC Configuration Example
--------

```C#
	//...
	_module.Bind<IFeatureSettingRepository<Feature>>()
		.To<AppConfigFeatureSettingRepository<Feature>>();
	_module.Bind<IFeatureSettingAvailabilityChecker<Feature>>()
		.ToMethod(x => new FeatureSettingAvailabilityChecker<Feature>((f,a) => true));
	_module.Bind<IFeatureSettingService<Feature>>()
		.To<FeatureSettingService<Feature>>();
	_module.Bind<IFeatureManifestCreationStrategy<Feature>>()
		.To<ManifestCreationStrategyDefault<Feature>>();
	_module.Bind<IFeatureManifestService<Feature>>()
		.To<FeatureManifestService<Feature>>();
	_module.Bind<IFeatureManifest<Feature>>()
		.ToMethod(x => _module.Kernel.Get<IFeatureManifestService<Feature>>().GetManifest());              
	//...
	
	
```

**Footnote 1:**
Please note that the logic to determine whether a feature is available is specified in the ```IFeatureManifestCreationStrategy``` instance you inject into the ```FeatureManifestService``` and (optionally, depending on your implementation of the aforementioned strategy) by the availability-checking function you inject into the ```FeatureSettingAvailabilityChecker```. 

Two concrete implementations of ```IFeatureManifestCreationStrategy``` are provided of-the-box: ```ManifestCreationStrategyDefault``` and ```ManifestCreationStrategyConsideringStateCookieTenantAndTime```. A single default availability checker function is provided out of the box ```DefaultFunctions.AvailabilityCheckFunction```, which may be used when the feature state, tenant, feature visibility mode and system time are known.

**Footnote 2:**
Marking a feature as established changes the behavior of the feature in the following way:

 - all dependencies must be established
 - checking the feature's availability will throw an exception (because it is now always available by deinition)
 - 

**NOTE: this is pre-release quality software. There will be bugs/inaccuracies in the documentation. If you find an issue, please help me by adding an issue here on GitHub.**