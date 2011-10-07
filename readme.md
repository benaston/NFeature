NFeature
====

A simple feature configuration system. 

Feature configuration walls enable you to integrate your code earlier, which brings lots of goodness (such as helping to avoid branch merge problems.)

How to use:
--------
**1. Define some features**

In your code:

```C#

	
    public enum Features
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

**2. Add code that is conditional on feature availability**
	
```C#


	if(Feature.MyFeature.IsAvailable(featureManifest)) //featureManifest ideally supplied via IOC container
	{
		//do some cool stuff
	}
	
```

**3. Configure feature dependencies**

```XML

	
    <features>
		<add name="MyFeature" dependencies="MyOtherFeature,MyOtherOtherFeature" />
	</features>

```

**4. Optionally configure feature settings using Json (neatly side-stepping the Microsoft XML configuration functionality)**
	
```XML

	
    <features>
		<add name="MyFeature" settings="{ mySetting:'mySettingValue', 
										  myOtherSetting:'myOtherSettingValue' }" />
	</features>

```

**5. Optionally specify dates for feature availability**

```XML

	
    <features>
		<add name="MyFeature" startDtg="23/03/2012:00:00:00" /> <!-- available from 23rd March 2012 forever -->
		<add name="MyOtherFeature" startDtg="23/03/2012:00:00:00" endDtg="24/03/2012:00:00:00" /> <!-- available from 23rd March 2012 until the 24th -->
		<add name="MyOtherOtherFeature" endDtg="24/03/2012:00:00:00" /> <!-- available until 24th March 2012 -->
	</features>

```

**6. Optionally mark your feature as ```Established``` to indicate that it is now integral to your application**

```XML

	
	<features>
		<add name="MyFeature" state="Established" />
	</features>

```

**7. Bask in the win**

How to build and/or run the tests:
--------

1. Run `/build/build.bat`
1. Type in the desired option
1. Hit return