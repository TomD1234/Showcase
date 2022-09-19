from time import sleep
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from woocommerce import API
from selenium.common.exceptions import NoSuchElementException

wcapi = API(
  url="http://test.local/",
  consumer_key="ck_74f2fd66640ff1549b73e52bb56e7775563e7692",
  consumer_secret="cs_cda49e74ce75946c4f394398c480eea68ce0dffe"
)

PATH=Service("C:/Users/tomde/eclipse-workspace/ChromeDriver/chromedriver.exe")

options = webdriver.ChromeOptions()
options.add_argument(f'user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36')
options.add_argument('--disable-blink-features=AutomationControlled')
options.add_experimental_option("excludeSwitches", ["enable-automation"])
options.add_experimental_option('useAutomationExtension', False)
options.add_argument("window-size=1980,1080")
options.add_argument("--incognito")
driver = webdriver.Chrome(service=PATH, options=options)
driver.execute_script("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})")

driver.get("https://www.collectandgo.be/colruyt/nl/assortiment/poly-palette?rootCategoryId=20016")
sleep(5)
print(driver.title)

category_data = {
    "name": "Example category 6"
}

response = wcapi.post("products/categories", category_data)

subcategory_data = {
    "name": "Test subcategory",
    "parent": response.json()["id"]
}
response = wcapi.post("products/categories", subcategory_data)
cat_teller=14
def FullDataScraper():
    #Checken of er wel producten in die categorie bestaan
    if str(driver.find_element(By.ID,"plpProducts").text)!="In het online assortiment van jouw afhaalpunt zitten geen producten van deze categorie. Onze excuses voor het ongemak!":
        #alle producten tonen
        try:
            driver.find_element(By.CLASS_NAME, "plp-item.--aditional")
            extra=1
        except NoSuchElementException:
            extra=0
        
        while int(driver.find_element(By.XPATH,"//span[@class='plp__product-count-showing']").text)<(int(driver.find_element(By.XPATH, "//span[@class='plp__product-count-total']").text)-extra):
            driver.execute_script("arguments[0].click();",driver.find_element(By.XPATH, "//button[@id='showMore']"))
            sleep(2)

        #Nu elk product webscrapen en meteen in je wordpress website zetten
        for product in driver.find_elements(By.CLASS_NAME,"l-grid__item.--col-xs-12.--col-sm-6.--col-lg-4.--col-xl-3.u-mb-15"):
            SKU=product.find_element(By.CLASS_NAME,"plp-item-top").get_attribute("data-sku")
            print("Product met SKU:"+SKU)
            
            #Checken of het product niet al bestaat adhv SKU
            if len(wcapi.get("products", params={'sku':SKU}).json())==1:
                print("Dit item bestaat al. Categorie wordt ge-updated")
                #zoek product id en parent ids
                dubbel_id=wcapi.get("products", params={'sku':SKU}).json()[0]["id"]
                parent_categories=wcapi.get("products", params={'sku':SKU}).json()[0]["categories"][0:]
                #Updaten categorie: oude + nieuwe
                data = {
                    "categories": parent_categories+[
                        {
                            "id": response.json()["id"]
                        }
                    ]
                }
                wcapi.put("products/{}".format(dubbel_id), data)
            else:
                info=[]
                for line in product.text.splitlines():
                    info.append(line)
                #print(info)
                if info[0]=="NIEUW":
                    info.pop(0)
                if info[0].isupper()==False:
                    info.insert(0, '<p style="font-size: 1em;"> </p>')
                if info[0].isupper()==True and info[1]==b"\x80".decode("windows-1252"):
                    info.insert(1,"")
                if info[2]==b"\x80".decode("windows-1252"):
                    if info[5]=="/st":
                        info.insert(2,"")
                    else:
                        info.insert(2,"blabla")
                if info[2]=="los" and info[6]=="/kg" or info[2]=="Los" and info[6]=="/kg" or info[2]=="(los)" and info[6]=="/kg":
                    info[2]="Los, 1kg"
                if info[1]=="Tijdelijk onbeschikbaar" or info[2]=="Tijdelijk onbeschikbaar" or info[3]=="Tijdelijk onbeschikbaar" or info[1]=="Einde seizoen" or info[2]=="Einde seizoen" or info[3]=="Einde seizoen":
                    continue
                if info[6]=="/kg" and info[2]!="Los, 1kg":
                    #print(info)
                    try:
                        gewicht=product.find_element(By.CLASS_NAME,"add-to-basket__item.--piece").get_attribute("data-step")
                        if gewicht=="100g":
                            gewicht=100
                    except NoSuchElementException:
                        gewicht=1000
                    gewicht=float(gewicht)
                    #print(round(gewicht/1000, 1))
                    gewicht=round(gewicht/1000, 2)
                    prijs_per_kg=float((info[4]+info[5]).replace(",","."))
                    prijs_per_st=round(gewicht*prijs_per_kg,2)
                    #print(str(prijs_per_st).split("."))
                    info[2]=u"\u00B1"+str(gewicht).replace(".", ",")+"kg"
                    info[4]=str(prijs_per_st).split(".")[0]+","
                    info[5]=str(prijs_per_st).split(".")[1]
                    info[6]="/st"
                    #print(info)
                imagelink=product.find_element(By.CLASS_NAME,"plp-item-top__image.product-image-{}".format(SKU)).get_attribute("src").replace("200x200","500x500")
                productlink=product.find_element(By.CLASS_NAME, "plp-item-top__image-container.product-link").get_attribute("href")
                driver.execute_script('''window.open("{}","_blank");'''.format(productlink))
                sleep(3)
                driver.switch_to.window(driver.window_handles[1])
                #Hier zijn we op de tab waar we de nutri-score kunnen vinden
                try:
                    NutriScore=driver.find_element(By.CLASS_NAME,"u-mt-15").find_element(By.XPATH, "./child::*").get_attribute("src")[-5]
                except NoSuchElementException:
                    NutriScore="Geen Nutri-Score beschikbaar"
                
                try:
                    driver.execute_script("arguments[0].click();",driver.find_element(By.LINK_TEXT, "Meer productinformatie"))
                    sleep(2)
                    driver.switch_to.window(driver.window_handles[2])
                    if str(driver.find_element(By.CLASS_NAME, "prod-info").text)!="Barcode    ":
                        #print("extra informatie beschikbaar")
                        try:
                            driver.execute_script("arguments[0].click();",driver.find_element(By.XPATH, "//a[contains(text(),'Voedingswaarden')]"))
                            sleep(1)
                            voedingswaarden=[]
                            for line in driver.find_element(By.CLASS_NAME, "row.value-container").text.splitlines():
                                voedingswaarden.append(line)
                            #print(voedingswaarden)
                            sleep(0.3)
                        except NoSuchElementException:
                            #print("Tabje is er wel, maar er staat niets...")
                            voedingswaarden="Geen voedingswaarden beschikbaar"
                    else:
                        #print("Wel een link, maar geen extra informatie")
                        voedingswaarden="Geen voedingswaarden beschikbaar"
                    driver.close()
                except NoSuchElementException:
                    #print("Geen extra productinformatie link")
                    voedingswaarden="Geen voedingswaarden beschikbaar"
                
                sleep(1)
                driver.switch_to.window(driver.window_handles[1])
                driver.close()
                driver.switch_to.window(driver.window_handles[0])
                sleep(0.1)
                print(info)
                print(NutriScore)
                print(voedingswaarden)
                #Nu hebben we alle gegevens en kunnen we ons product maken
                if NutriScore=="Geen Nutri-Score beschikbaar":
                    if len(voedingswaarden)==0 or voedingswaarden=="Geen voedingswaarden beschikbaar":
                        product_data = {
                            "name": info[0],
                            "regular_price": info[4].replace(",", ".")+info[5],
                            "short_description": info[1]+"\n"+info[2]+" "+info[7],
                            "description": "Geen Nutri-Score beschikbaar",
                            "sku": SKU,
                            "categories": [
                              {
                                "id": response.json()["id"]
                              }
                            ],
                        
                            "images": [
                                {
                                    "src": imagelink
                                }
                            ],
                            "attributes": [
                                {
                                    "name": ":",
                                    "visible": True,
                                    "options": ["Geen voedingswaarde beschikbaar"]
                                }
                            ]
                        }
                    else:
                        #Zorgen dat de voedingswaarden even lang zijn (soms vezels erbij soms niet)
                        while len(voedingswaarden)<24:
                            voedingswaarden.append("")
                                            
                        product_data = {
                            "name": info[0],
                            "regular_price": info[4].replace(",", ".")+info[5],
                            "short_description": info[1]+"\n"+info[2]+" "+info[7],
                            "sku": SKU,
                            "description": "Geen Nutri-Score beschikbaar",
                            "categories": [
                              {
                                "id": response.json()["id"]
                              }
                            ],
                        
                            "images": [
                                {
                                    "src": imagelink
                                }
                            ],
                            "attributes": [
                                {
                                    "name": voedingswaarden[0],
                                    "visible": True,
                                    "options": [voedingswaarden[1]]
                                },
                                {
                                    "name": voedingswaarden[2],
                                    "visible": True,
                                    "options": [voedingswaarden[3]]
                                },
                                {
                                    "name": voedingswaarden[4],
                                    "visible": True,
                                    "options": [voedingswaarden[5]]
                                },
                                {
                                    "name": voedingswaarden[6],
                                    "visible": True,
                                    "options": [voedingswaarden[7]]
                                },
                                {
                                    "name": voedingswaarden[8],
                                    "visible": True,
                                    "options": [voedingswaarden[9]]
                                },
                                {
                                    "name": voedingswaarden[10],
                                    "visible": True,
                                    "options": [voedingswaarden[11]]
                                },
                                {
                                    "name": voedingswaarden[12],
                                    "visible": True,
                                    "options": [voedingswaarden[13]]
                                },
                                {
                                    "name": voedingswaarden[14],
                                    "visible": True,
                                    "options": [voedingswaarden[15]]
                                },
                                {
                                    "name": voedingswaarden[16],
                                    "visible": True,
                                    "options": [voedingswaarden[17]]
                                },
                                {
                                    "name": voedingswaarden[18],
                                    "visible": True,
                                    "options": [voedingswaarden[19]]
                                },
                                {
                                    "name": voedingswaarden[20],
                                    "visible": True,
                                    "options": [voedingswaarden[21]]
                                },
                                {
                                    "name": voedingswaarden[22],
                                    "visible": True,
                                    "options": [voedingswaarden[23]]
                                }
                            ]
                        }
                #De elif loop is voor https://www.collectandgo.be/colruyt/nl/assortiment/sla-bladgroenten?rootCategoryId=20001#pdp_3074457345616706101
                elif len(voedingswaarden)==0 or voedingswaarden=="Geen voedingswaarden beschikbaar":
                    product_data = {
                        "name": info[0],
                        "regular_price": info[4].replace(",", ".")+info[5],
                        "short_description": info[1]+"\n"+info[2]+" "+info[7],
                        "sku": SKU,
                        "description": '<img src="http://test.local/wp-content/uploads/2021/11/Nutri-{}.png" width="125"/>'.format(str(NutriScore)),
                        "categories": [
                          {
                            "id": response.json()["id"]
                          }
                        ],
                    
                        "images": [
                            {
                                "src": imagelink
                            }
                        ],
                        "attributes": [
                            {
                                "name": ":",
                                "visible": True,
                                "options": ["Geen voedingswaarde beschikbaar"]
                            }
                        ]
                    }
                else:
                    #Zorgen dat de voedingswaarden even lang zijn (soms vezels erbij soms niet)
                    while len(voedingswaarden)<24:
                        voedingswaarden.append("")
                    
                    product_data = {
                        "name": info[0],
                        "regular_price": info[4].replace(",", ".")+info[5],
                        "short_description": info[1]+"\n"+info[2]+" "+info[7],
                        "sku": SKU,
                        "description": '<img src="http://test.local/wp-content/uploads/2021/11/Nutri-{}.png" width="125"/>'.format(str(NutriScore)),
                        "categories": [
                          {
                            "id": response.json()["id"]
                          }
                        ],
                    
                        "images": [
                            {
                                "src": imagelink
                            }
                        ],
                        "attributes": [
                            {
                                "name": voedingswaarden[0],
                                "visible": True,
                                "options": [voedingswaarden[1]]
                            },
                            {
                                "name": voedingswaarden[2],
                                "visible": True,
                                "options": [voedingswaarden[3]]
                            },
                            {
                                "name": voedingswaarden[4],
                                "visible": True,
                                "options": [voedingswaarden[5]]
                            },
                            {
                                "name": voedingswaarden[6],
                                "visible": True,
                                "options": [voedingswaarden[7]]
                            },
                            {
                                "name": voedingswaarden[8],
                                "visible": True,
                                "options": [voedingswaarden[9]]
                            },
                            {
                                "name": voedingswaarden[10],
                                "visible": True,
                                "options": [voedingswaarden[11]]
                            },
                            {
                                "name": voedingswaarden[12],
                                "visible": True,
                                "options": [voedingswaarden[13]]
                            },
                            {
                                "name": voedingswaarden[14],
                                "visible": True,
                                "options": [voedingswaarden[15]]
                            },
                            {
                                "name": voedingswaarden[16],
                                "visible": True,
                                "options": [voedingswaarden[17]]
                            },
                            {
                                "name": voedingswaarden[18],
                                "visible": True,
                                "options": [voedingswaarden[19]]
                            },
                            {
                                "name": voedingswaarden[20],
                                "visible": True,
                                "options": [voedingswaarden[21]]
                            },
                            {
                                "name": voedingswaarden[22],
                                "visible": True,
                                "options": [voedingswaarden[23]]
                            }
                        ]
                    }
                wcapi.post('products', product_data)
            sleep(0.3)
            print("Product klaar.")
    driver.execute_script("window.history.go(-1)")
    sleep(1)

FullDataScraper()