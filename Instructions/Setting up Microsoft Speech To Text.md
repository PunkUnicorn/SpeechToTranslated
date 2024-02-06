# Setting up Microsoft Speech To Text

## One of three steps required to setup SpeechToTranslated

### Github

Sign up to www.github.com, which is free. This is where you can get a copy of the SpeechToTranslated program.

Navigate to the web page http://www.github.com/PunkUnicorn/SpeechToTranslated

Click on 'Actions' (see picture)

![Microsoft Azure, Speech Services](Github, 1 - actions.png)

Select the top version (see picture)

![Microsoft Azure, Speech Services](Github, 2 - choose top build.png)

Then download the build results (see picture)

![Microsoft Azure, Speech Services](Github, 3 - download.png)

Unzip the file 'Build result.zip' into some folder.

Find the file called 'appsettings.json' because next we'll generate keys then we'll paste the keys into this file.


## Two of three steps required to setup SpeechToTranslated

### Azure - Speech Services

SpeechToTranslated uses Microsoft Azure "Speech Services".

Sign up with Microsoft Azure.

After you've signed up, go to the Azure Portal and search for "Speech Services", then click "Create new". There is a free tier of service, which is OK for a few hours of usage per month.

Creating a new Speech Service will bring up this screen. That will bring you to this screen. 

![Microsoft Azure, Speech Services](Azure Speech Services.png)

Click through until you've created your subscription. The defaults are fine past the above screen.

Then after your Speech Service subscription has been created select the overview of your new service, you will see something similar to this:

![Microsoft Azure, Speech Services Success](Azure Speech Services, Success.png)

Take a copy of your key and region.

Now find the file called 'appsettings.json' from the 'Build Results.zip'. Make sure you only replace the three dots and the quotation marks are still there. So your key (and region) are enclosed by the double quotation marks.

Copy and paste the key and 'region' into the appsettings file:

![Microsoft Azure, Speech Services Success](appsettings, 1 - Azure Bits.png)


## Three of three steps required to setup SpeechToTranslated

### DeepL API - translation service

SpeechToTranslated uses DeepL API, a translation web service.

Sign up for DeepL Api, but watch out! There are many types of DeepL subscription but we want 'DeepL Api' (see picture)

![Microsoft Azure, Speech Services Success](DeepL API, 1.png)

Click 'sign up for free':

![Microsoft Azure, Speech Services Success](DeepL API, 2 - sign up for free.png)

Choose the free plan:

![Microsoft Azure, Speech Services Success](DeepL API, 3 - free plan.png)

After you've signed up for the free plan, go to your account settings to get your DeepL key

![Microsoft Azure, Speech Services Success](DeepL API, 4 - account.png)

Then copy your key and paste it into the file 'appsettings.json'. Make sure you only replace the three dots and the quotation marks are still there. So your key is enclosed by the double quotation marks.

![Microsoft Azure, Speech Services Success](DeepL API, 5 - key.png)

