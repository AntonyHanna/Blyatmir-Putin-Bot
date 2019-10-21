[![Codacy Badge](https://api.codacy.com/project/badge/Grade/85614d329f784c5685256e0bd5589699)](https://www.codacy.com/manual/AntonyHanna/Blyatmir-Putin-Bot?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=AntonyHanna/Blyatmir-Putin-Bot&amp;utm_campaign=Badge_Grade)
[![Github Repo Size](https://img.shields.io/github/release-pre/antonyhanna/blyatmir-putin-bot.svg)](https://github.com/AntonyHanna/Blyatmir-Putin-Bot/releases/tag/v0.1.0)
[![Github Repo Size](https://img.shields.io/github/repo-size/AntonyHanna/Blyatmir-Putin-Bot.svg?style=popout)](https://github.com/AntonyHanna/Blyatmir-Putin-Bot)
[![Docker Build Status](https://img.shields.io/docker/cloud/build/antonyhanna/blyatmir-putin-bot.svg?style=popout)](https://cloud.docker.com/repository/docker/antonyhanna/blyatmir-putin-bot)


Blyatmir Putin Bot <img align="right" width="100" height="100" src="https://cdn.discordapp.com/attachments/559700127275679762/591566828367642635/dd321c999e478a17136a288dd15144e2.png">
======================
Once there was a boy named Blyatmir. He was a simple man with big dreams, dreams of uniting the savage lands of the Russia. Through hard work and compassion and the power of communism, the young tyke gained influence in his local village of Slavensk. He rose to power and led a revolution in Slavensk, introducing the wonders of communism and publishing his life's work, *The Blyat Manifesto*. As our young Blyatmir grew older, his power and influence grew and he united the lands of Russia under one banner, the UBR.

After years of prosperity, rumours stirred of the freedom man of the west, with promises of prosperous lands of vodka and adidas. Blyatmir, now with his true title of Putin, declared war on the freedom man and labeled him as an enemy of the people. As the freedom man further challenges our Blyat, he must fight for his people, his vodka and for his Slavenskis. This is the story of Blyatmir Putin.
 
 Abilities <img align="left" width="40" height="40" src="https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/209/man-mage_1f9d9-200d-2642-fe0f.png">
 ---------
 **What can our comrade Blyatmir do?**<img align="left" width="25" height="25" src="https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/209/clipboard_1f4cb.png">
  - Purge messages so no-one ever knows of our other comrades secrets
  - Sympathise with our fallen bretheren through the use of `F`
  - Block the language of the furries
  - Rate how Blyatmir is doing on his quest for revenge
  - Tell people to blyat
  - Rate your server as compared to others
  - Eventually Blyatmir will sort out his vodka addiction and learn to do more things
  
 **What will Blyatmir be able to do once he is no longer drunk?** <img align="left" width="25" height="25" src="https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/microsoft/209/cocktail-glass_1f378.png">
  - Hold conversations with you like a proper comrade 
  - Play music for you
  - Start and Stop game servers for you
  - Quote messages for you
  - Mention people about the stupid shit they said about Blyatmir
  
  ___

### Running the bot via docker <img align="left" width="50" height="30" src="https://cdn.discordapp.com/attachments/559700127275679762/591585009358471182/docker-whale-home-logo.png">

Required variables are marked with a `*`

| Var-Type |   Name          |  Type   |  Default Value  |  Description  |      |
| :-:      |  ------         | :----:  | :-------------: | ------------- | :--: |
| -e       | BOT_TOKEN       | string  | null    | Your discord applications bot token | * |
| -e       | BOT_PREFIX      | char    | null    | Your desired prefix for bot commands| * |
| -e       | BOT_ACTIVITY    | string  | null    | Your desired activity for the bot to display| |
| -e       | DOCKER_IP       | string  | null    | The IP of a device running a docker instance | |
| -e       | SERVER_LOGIN    | string  | null    | The username of an account with the ability to ssh in the docker instance | |
| -e       | SERVER_PASSWORD | string  | null    | The password the account specified | |
| -v       | /path/on/host:/Data       | string  | null    | Path on host to where you want to store config and data files | * |

##### Minimal Configuration
```docker
> docker run -v /path/on/host:/Data -e BOT_TOKEN=YOUR_BOT_TOKEN_HERE -e BOT_PREFIX=DESIRED_PREFIX_HERE \
  antonyhanna/blyatmir-putin-bot
```
##### Full Configuration
```docker
> docker run -v /path/on/host:/Data -e BOT_TOKEN=YOUR_BOT_TOKEN_HERE -e BOT_PREFIX=DESIRED_PREFIX_HERE \
  -e BOT_ACTIVITY=DESIRED_BOT_ACTIVITY_HERE -e DOCKER_IP=DOCKER_INSTANCE_IP -e SERVER_LOGIN=USERNAME \
  -e SERVER_PASSWORD=PASSWORD antonyhanna/blyatmir-putin-bot
```
  
  
  
  

   
  
