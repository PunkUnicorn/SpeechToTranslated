# Setting up Microsoft Speech To Text

## One of three steps required to setup SpeechToTranslated

### Github

Sign up to www.github.com, which is free. This is where you can get a copy of the SpeechToTranslated program.

Navigate to the web page http://www.github.com/PunkUnicorn/SpeechToTranslated

Click on 'Actions' (see picture)

![Microsoft Azure, Speech Services](Github,%201%20-%20actions.png)

Select the top version (see picture)

![Microsoft Azure, Speech Services](Github,%202%20-%20choose%20top%20build.png)

Then download the build results (see picture)

![Microsoft Azure, Speech Services](Github,%203%20-%20download.png)

Unzip the file 'Build result.zip' into some folder.

Find the file called 'appsettings.json' because next we'll generate keys then we'll paste the keys into this file.


## Two of three steps required to setup SpeechToTranslated

### Azure - Speech Services

SpeechToTranslated uses Microsoft Azure "Speech Services".

Sign up with Microsoft Azure.

After you've signed up, go to the Azure Portal and search for "Speech Services", then click "Create new". There is a free tier of service, which is OK for a few hours of usage per month.

Creating a new Speech Service will bring up this screen. That will bring you to this screen. 

![Microsoft Azure, Speech Services](Azure%20Speech%20Services.png)

Click through until you've created your subscription. The defaults are fine past the above screen.

Then after your Speech Service subscription has been created select the overview of your new service, you will see something similar to this:

![Microsoft Azure, Speech Services Success](Azure%20Speech%20Services,%20Success.png)

Take a copy of your key and region.

Now find the file called 'appsettings.json' from the 'Build Result.zip'. Make sure you only replace the three dots and the quotation marks are still there. So your key (and region) are enclosed by the double quotation marks.

Copy and paste the key and 'region' into the appsettings file:

![Microsoft Azure, Speech Services Success](appsettings,%201%20-%20Azure Bits.png)


## Three of three steps required to setup SpeechToTranslated

### DeepL API - translation service

SpeechToTranslated uses DeepL API, a translation web service.

Sign up for DeepL Api, but watch out! There are many types of DeepL subscription but we want 'DeepL Api' (see picture)

![Microsoft Azure, Speech Services Success](DeepL%20API,%201.png)

Click 'sign up for free':

![Microsoft Azure, Speech Services Success](DeepL%20API,%202%20-%20sign%20up%20for%20free.png)

Choose the free plan:

![Microsoft Azure, Speech Services Success](DeepL%20API,%203%20-%20free%20plan.png)

After you've signed up for the free plan, go to your account settings to get your DeepL key

![Microsoft Azure, Speech Services Success](DeepL%20API,%204%20-%20account.png)

Then copy your key and paste it into the file 'appsettings.json'. Make sure you only replace the three dots and the quotation marks are still there. So your key is enclosed by the double quotation marks.

![Microsoft Azure, Speech Services Success](DeepL%20API,%205%20-%20key.png)


# Ready to go!

Now you can run SpeechToTranslated.exe from wherever you extracted the contents of 'Build result.zip' to.

It's designed to be run from the console, and it takes parameters so you can specify the languages you want to translate into.

For example:

./SpeechToTranslated.exe pl es bg

...will run the speech translator and translate into Polish (pl), Spanish (es), and Bulgarian (bg). The codes are the internation standard codes.

Also for example:

./SpeechToTranslated.exe bg en-GB

...will translate into Bulgarian and English
