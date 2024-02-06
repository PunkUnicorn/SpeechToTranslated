```    _____                      _                  
  / ____|                    | |                 
 | (___  _ __   ___  ___  ___| |__               
  \___ \| '_ \ / _ \/ _ \/ __| '_ \              
  ____) | |_) |  __/  __/ (__| | | |             
 |_____/| .__/ \___|\___|\___|_| |_|             
        | |                                      
  ______|_|                                      
 |__   __|                                       
    | | ___                                      
    | |/ _ \                                     
    | | (_) |                                    
  __|_|\___/               _       _           _ 
 |__   __|                | |     | |         | |
    | |_ __ __ _ _ __  ___| | __ _| |_ ___  __| |
    | | '__/ _` | '_ \/ __| |/ _` | __/ _ \/ _` |
    | | | | (_| | | | \__ \ | (_| | ||  __/ (_| |
    |_|_|  \__,_|_| |_|___/_|\__,_|\__\___|\__,_|
``` 


# One of three steps required to setup SpeechToTranslated

## Github

Sign up to www.github.com, which is free. This is where you can get a copy of the SpeechToTranslated program.

Navigate to the web page http://www.github.com/PunkUnicorn/SpeechToTranslated

Click on 'Actions' (see picture)

![](Github,%201%20-%20actions.png)

Select the top version (see picture)

![](Github,%202%20-%20choose%20top%20build.png)

Then download the build results (see picture)

![](Github,%203%20-%20download.png)

Unzip the file 'Build result.zip' into some folder.

Find the file called 'appsettings.json' because next we'll generate keys then we'll paste the keys into this file.


# Two of three steps required to setup SpeechToTranslated

## Azure - Speech Services

SpeechToTranslated uses Microsoft Azure "Speech Services".

Sign up with Microsoft Azure.

After you've signed up, go to the Azure Portal and search for "Speech Services", then click "Create new". There is a free tier of service, which is OK for a few hours of usage per month.

Creating a new Speech Service will bring up this screen. That will bring you to this screen. 

![](Azure%20Speech%20Services.png)

Click through until you've created your subscription. The defaults are fine past the above screen.

Then after your Speech Service subscription has been created select the overview of your new service, you will see something similar to this:

![](Azure%20Speech%20Services,%20Success.png)

Take a copy of your key and region.

Now find the file called 'appsettings.json' from the 'Build Result.zip'. Make sure you only replace the three dots and the quotation marks are still there. So your key (and region) are enclosed by the double quotation marks.

Copy and paste the key and 'region' into the appsettings file:

![](appsettings,%201%20-%20Azure%20Bits.png)


# Three of three steps required to setup SpeechToTranslated

## DeepL API - Translation Service

SpeechToTranslated uses DeepL API, a translation web service.

Sign up for DeepL Api, but watch out! There are many types of DeepL subscription but we want 'DeepL Api' (see picture)

![](DeepL%20API,%201.png)

Click 'sign up for free':

![](DeepL%20API,%202%20-%20sign%20up%20for%20free.png)

Choose the free plan:

![](DeepL%20API,%203%20-%20free%20plan.png)

After you've signed up for the free plan, go to your account settings to get your DeepL key

![](DeepL%20API,%204%20-%20account.png)

Take a copy of your DeepL key.

![](DeepL%20API,%205%20-%20key.png)

Then copy your key into the file 'appsettings.json'. Make sure you only replace the three dots and the quotation marks are still there. So your key is enclosed by the double quotation marks.

![](appsettings,%202%20-%20DeepL%20key.png)

In 'appsettings.json' the google key is ignored.

Be careful not to put your keys anywhere public! If you accidentally make your keys public it is possile to generate new keys to replace your old ones.


# Ready to go!

Now you can run SpeechToTranslated.exe from wherever you extracted the contents of 'Build result.zip' to.

It's designed to be run from the console, and it takes parameters so you can specify the languages you want to translate into.

For example:

./SpeechToTranslated.exe pl es bg

...will run the speech translator and translate into Polish (pl), Spanish (es), and Bulgarian (bg). The codes are the internation standard codes.

Also for example:

./SpeechToTranslated.exe bg en-GB

...will translate into Bulgarian and English
