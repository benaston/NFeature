NFeature
====

A simple feature configuration system. 

Feature configuration walls enable you to integrate your code earlier, which brings lots of goodness (such as helping to avoid branch merge problems.)

**NOTE: this is pre-release quality software. There will be bugs/inaccuracies in the documentation. If you find an issue, please help me by letting me know at ```ben@bj.ma```.**

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

In your configuration:

```XML

	
    <features>
		<add name="MyFeature" state="Enabled" /> <!-- will be available to all -->
		<add name="MyOtherFeature" state="Previewable" /> <!-- will only be available to users who meet the feature-preview criteria* -->
		<add name="MyOtherOtherFeature" state="Disabled" /> <!-- not available -->
	</features>

	<!-- *the default feature-preview criteria is the presence of a cookie on the client, but this is pluggable functionality -->
	
```

**2. Take care of feature manifest initialization**

Note that the availability checker is injected with a method that can contain arbitrary logic. You might, for example, take advantage of this to disable certain features when the load on a website is above normal.

```C#

	
	//NOTE: I suggest hiding this all away in the IOC container configuration	
	var availabilityChecker = new FeatureSettingAvailabilityChecker<Feature, TArgs>(MyAvailabilityCheckingMethod);
	var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature>();
	var featureSettingService = new FeatureSettingService<Feature, TArgs>(availabilityChecker, featureSettingRepo);
	//NOTE: args here is an instance of TArgs, used to supply information to the availability checker
	var manifestCreationStrategy = new CookieBasedPreviewManifestCreationStrategy<Feature>(featureSettingService, featureSettingRepo, args);
	var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
	var featureManifest = featureManifestService.GetManifest();


```

**3. Add code that is conditional on feature availability**
	
```C#


	if(Feature.MyFeature.IsAvailable(featureManifest)) //featureManifest ideally supplied via IOC container
	{
		//do some cool stuff
	}
	
```

**4. Configure feature dependencies**

```XML

	
    <features>
		<add name="MyFeature" dependencies="MyOtherFeature,MyOtherOtherFeature" />
	</features>

```

**5. Optionally configure feature settings using Json (neatly side-stepping the Microsoft XML configuration functionality)**
	
```XML

	
    <features>
		<add name="MyFeature" settings="{ mySetting:'mySettingValue', 
										  myOtherSetting:'myOtherSettingValue' }" />
	</features>

```

**6. Optionally specify dates for feature availability**

```XML

	
    <features>
		<add name="MyFeature" startDtg="23/03/2012:00:00:00" /> <!-- available from 23rd March 2012 forever -->
		<add name="MyOtherFeature" startDtg="23/03/2012:00:00:00" endDtg="24/03/2012:00:00:00" /> <!-- available from 23rd March 2012 until the 24th -->
		<add name="MyOtherOtherFeature" endDtg="24/03/2012:00:00:00" /> <!-- available until 24th March 2012 -->
	</features>

```

**7. Optionally mark your feature as ```Established``` to indicate that it is now integral to your application**

```XML

	
	<features>
		<add name="MyFeature" state="Established" />
	</features>

```

**8. Bask in the win**

How to build and/or run the tests:
--------

1. Run `/build/build.bat`
1. Type in the desired option
1. Hit return