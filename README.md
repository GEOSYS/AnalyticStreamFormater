<div id="top"></div>
<!-- PROJECT SHIELDS -->
<!--
*** See the bottom of this document for the declaration of the reference variables
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->


<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href=https://github.com/GEOSYS>
    <img src=https://earthdailyagro.com/wp-content/uploads/2022/01/Logo.svg alt="Logo" width="400" height="200">
  </a>

  <h1 align="center">Analytic Stream Formater</h3>

  <p align="center">
    Enable the processing of event coming from '<geosys/> to get, format and publish analytic on an Azure storage account.
    <br />
    <a href=https://earthdailyagro.com/><strong>Who we are</strong></a>
    <br />
    <br />
    <a href=https://github.com/GEOSYS/AnalyticStreamFormater>Project description</a>
    ·
    <a href=https://github.com/GEOSYS/AnalyticStreamFormater/issues>Report Bug</a>
    ·
    <a href=https://github.com/GEOSYS/AnalyticStreamFormater/issues>Request Feature</a>
  </p>
</p>

<div align="center">
  
[![LinkedIn][linkedin-shield]][linkedin-url]
[![Twitter][twitter-shield]][twitter-url]
[![Youtube][youtube-shield]][youtube-url]
<!--[![languages][language-python-shiedl]][]-->
[![languages][NETcore-shield]][NETcore-url]
<!--[![CITest][CITest-shield]][CITest-url]-->
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
  
</div>

<!--[![Stargazers][GitStars-shield]][GitStars-url]-->
<!--[![Forks][forks-shield]][forks-url]-->
<!--[![Stargazers][stars-shield]][stars-url]-->

<!-- TABLE OF CONTENTS -->
<details open>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#features">Features</a></li>
    <li><a href="#use-cases">Use cases</a></li>
    <li><a href="#support-development">Support development</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#copyrights">Copyrights</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

EarthDaily Agro is the agricultural analysis division of EartDaily Analytics. Learn more about Earth Daily at [EarthDaily Analytics | Satellite imagery & data for agriculture, insurance, surveillance](https://earthdaily.com/).  EarthDaily Agro uses satellite imaging to provide advanced analytics to mitigate risk and increase efficiencies – leading to more sustainable outcomes for the organizations and people who feed the planet.
<p align="center">
  <a href=https://earthdailyagro.com/geosys/>
    <img src=https://earthdailyagro.com/wp-content/uploads/2022/01/new-logo.png alt="Logo" width="400">
  </a>
</p>

Throught our <geosys/> platform, we make geospatial analytics easily accessible for you to be browsed or analyzed, within our cloud or within your own environment. We provide developers and data scientists both flexibility and extensibility with analytic ready data and digital agriculture ready development blocks. We empower your team to enrich your systems with information at the field, regional or continent level via our API or Apps.

We have a team of experts around the world that understand local crops and ag industry, as well as advanced analytics to support your business.

We have established a developer community to provide you with plug-ins and integrations to be able to discover, request and use aggregate imagery products based on Landsat, Sentinel, Modis and many other open and commercial satellite sensors.

The current project aims to provide an easy and ready to use event consumer and analytic formater allowing customer to quickly initializze and maintain up to date field level analytic derived from satelite imagery.

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

Use of this project requires valids credentials from the '<geosys/> platform . If you need to get trial access, please register [here](https://earthdailyagro.com/geosys-api/#get-started).
This project is using .Net Core XXX

### Installation
To start replicate curent project and launch it using your code editor.

### Configuration
Update teh appseettings file: 

```
"Azure": { 
   "BlobStorage": {
    "ConnectionString": "MyAzureBlobStorageConnectionString" <- Put the connection string to your Microsoft Storage account }
    }, 
   "IdentityServer": { 
    "Url": "IdentityServerUrl", -> On veut target la préprod ou/et la prod ??? 
    "TokenEndPoint": "connect/token", 
    "UserLogin": "myuser", <- Set the user login get from your trial access here "UserPassword": 
    "mypassword", <- Set the password get from your trial access here 
    "ClientId": "myclientid", <- Set the clientId get from your trial access here "ClientSecret": 
    "myclientsecret", <- Set the clientSecret get from your trial access here 
    "Scope": "openid offline_access", "GrantType": "password" 
    }, 
    "MapProduct": { 
      "Url": "MapProductUrl"
     }
```
Finally build and deploy host this web project in your infrastructure.

### Connect to EarhtDaily Agro event stream
Contact Earthdaily Agro customer desk to share your formater URL. We will configure our notification pipeline to publish event to your URL.

### Subscribe fields
Each event from this stream is tied to a field and a new analytic becoming availabe (clear image or other analytic generated for this field). To enable analytic event publication, each field of interest has to be subsribed. 
This can we done using subscription API:
  - for historical analytics using 
 
  ```
  POST 'http://<root>/analytics-sink-subscriptions/v1/user-sink-entity-replays/query' \
--header 'Content-Type: application/json' \
--header 'Accept: */*' \
--header 'Authorization: Bearer _Token_' \
--data-raw '{
    "entity": {
        "id": "_SeasonField_id_",
        "type": "SeasonField"
    },
    "user": {
        "id": "_user_unique_id_"
    },
    "schema": {
        "id": "_SCHEMA_CODE_"
    }
}'
```
With this subcription, an event will be generated for each analytic corresponding to the Schema and tied to the field. This subcription will not trigger any analytic generation.

  - for real time analytics using

```
  POST 'http://<root>/analytics-sink-subscriptions/v1//user-sink-subscriptions' \
--header 'Content-Type: application/json' \
--header 'Accept: */*' \
--header 'Authorization: Bearer _Token_' \
--data-raw '{
    "entity": {
        "id": "_SeasonField_id_",
        "type": "SeasonField"
    },
    "user": {
        "id": "_user_unique_id_"
    },
    "schema": {
        "id": "_SCHEMA_CODE_"
    },
    "streaming": {
    "enabled": true
    }
}'
```
With this subcription, an event will be generated once a new analytic corresponding to the Schema and tied to the field is generated. This subcription will not trigger any analytic generation, it is publishing a notification event based on an internal generation event.

<p align="right">(<a href="#top">back to top</a>)</p>
   
<!-- FEATURES -->
## Features

### Standart features 

The Analytic Stream Formater will for each notification, fecth the analytic:
   - an NDVI map as a Geotiff zipped file
   - a change index json object

Each analytic will be published in a storage acccount configured. Each NDVI map will be a specific asset whereas change index values will be consolidated into a CSV file. 

<p align="center">
  <a href=https://github.com/GEOSYS>
    <img src=https://github.com/GEOSYS/Images/blob/main/AnalyticStreamFormater/StorageExplorer.jpg alt="Logo" width="800" height="400">
  </a>
</p>

The naming convention is <GeosysSeasonFieldID-yyyy-mm-dd-<imageID>.zip
  
### Customization
If you want to manage the integration of notification inside your platform, manage its persistence, you can update or implement your own version of the IAnalyticsService.

To change the map type extracted (Geotiff NDVI), please update or implement

We also provide consulting services in case you need support to create your own analytic pipeline. 

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
## Use cases

### Extract historical dataset on field

The Analytic stream Formatter can be used to initalize innovation project field level dataset. This will allow you to fectch 10+ years of clear maps on fields of interest.

### Maintain real time field analytic datase
The Analytic stream Formatter is the perfect app to create you own field level map pipeline allowing you to receive fresh and direclty usable field level analytic as they become available.

<!-- RESOURCES -->
## Resources 
The following links will provide access to more information:
- [EarthDaily agro developer portal  ](https://developer.geosys.com/)
- [Azure storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview)
- [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Support development

If this project has been useful, that it helped you or your business to save precious time, don't hesitate to give it a star.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the [GPL 3.0 License](https://www.gnu.org/licenses/gpl-3.0.en.html). 

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

For any additonal information, please <a href="mailto: sales@earthdailyagro.com">email us</a>

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- COPYRIGHT -->
## Copyrights

© 2022 Geosys Holdings ULC, an Antarctica Capital portfolio company | All Rights Reserved.

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
<!-- List of available shields https://shields.io/category/license -->
<!-- List of available shields https://simpleicons.org/ -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo.svg?style=social
[NETcore-shield]: https://img.shields.io/badge/.NET%20Core-6.0-green
[NETcore-url]: https://github.com/dotnet/core
[contributors-url]: https://github.com/github_username/repo/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo.svg?style=plastic&logo=appveyor
[forks-url]: https://github.com/github_username/repo/network/members
[stars-shield]: https://img.shields.io/github/stars/qgis-plugin/repo.svg?style=plastic&logo=appveyor
[stars-url]: https://github.com/github_username/repo/stargazers
[issues-shield]: https://img.shields.io/github/issues/GEOSYS/qgis-plugin/repo.svg?style=social
[issues-url]: https://github.com/github_username/repo/issues
[license-shield]: https://img.shields.io/github/license/GEOSYS/qgis-plugin
[license-url]: https://www.gnu.org/licenses/gpl-3.0.en.html
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=social&logo=linkedin
[linkedin-url]: https://www.linkedin.com/company/earthdailyagro/mycompany/
[twitter-shield]: https://img.shields.io/twitter/follow/EarthDailyAgro?style=social
[twitter-url]: https://img.shields.io/twitter/follow/EarthDailyAgro?style=social
[youtube-shield]: https://img.shields.io/youtube/channel/views/UCy4X-hM2xRK3oyC_xYKSG_g?style=social
[youtube-url]: https://img.shields.io/youtube/channel/views/UCy4X-hM2xRK3oyC_xYKSG_g?style=social
[language-python-shiedl]: https://img.shields.io/badge/python-3.7-green?logo=python
[language-python-url]: https://pypi.org/ 
[GitStars-shield]: https://img.shields.io/github/stars/GEOSYS?style=social
[GitStars-url]: https://img.shields.io/github/stars/GEOSYS?style=social
[CITest-shield]: https://img.shields.io/github/workflow/status/GEOSYS/qgis-plugin/Continous%20Integration
[CITest-url]: https://img.shields.io/github/workflow/status/GEOSYS/qgis-plugin/Continous%20Integration

