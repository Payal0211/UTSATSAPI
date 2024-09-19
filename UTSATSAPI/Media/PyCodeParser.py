import nltk
import string
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from geotext import GeoText
import re
from pathlib import Path


print ('Content-Type: text/plain')
print ('')

result=[]
fileCount = 0

my_file = Path("D:\\\TFSProject\\UTS\\TalentConnectAdminAPI\\Media\\JDParsing\\JDFiles\\File1.txt")
if my_file.is_file():
    fileCount = 1
    txtFile = open("D:\\\TFSProject\\UTS\\TalentConnectAdminAPI\\Media\\JDParsing\\JDFiles\\File1.txt",encoding="utf8")


if fileCount == 0:
    print(result)
else:
    FileText = txtFile.read()
    txtFile.close()

    FileText = FileText.replace('"','')

    FileText = FileText.replace("'","")
    FileText = re.sub('[^a-zA-Z0-9 \n\.]', ' ', FileText)
    psText = FileText.split(' ')
    UCasedFileText = ""
    for word in psText:
        if not word.isupper():
            UCasedFileText =UCasedFileText + word.capitalize() + " "
        else:
            UCasedFileText = UCasedFileText + word + " "

    FileText = UCasedFileText


    '''
    THIS BLOCK REMOVES ALL CITIES/COUNTRY NAMES/COUNTRY MENTIONS FROM JD
    ***START***
    '''
    GeoCityReplacedText = ""
    GeoCountryReplacedText = ""
    GeoCountryMentionsReplacedText = ""

    places = GeoText(FileText)
    if len(places.cities) > 0:
        for tcity in places.cities:
            GeoCityReplacedText = FileText.replace(tcity," ")

        FileText = GeoCityReplacedText


    if len(places.countries) > 0:
        for tcountry in places.countries:
            GeoCountryReplacedText = FileText.replace(tcountry," ")

        FileText = GeoCountryReplacedText

    '''
    THIS BLOCK REMOVES ALL CITIES/COUNTRY NAMES/COUNTRY MENTIONS FROM JD
    ***END***
    '''



    '''
    THIS BLOCK REMOVES ALL NUMBERED STRINGS FROM LIST EXTRACTED FROM JD
    ***START***
    '''
    skill_list = []
    skill_list = list(FileText.split(" "))
    skill_list = [x for x in skill_list if not any(c.isdigit() for c in x)]
    FileText = ' '.join(map(str,skill_list))
    '''
    THIS BLOCK REMOVES ALL NUMBERED STRINGS FROM LIST EXTRACTED FROM JD
    ***END***
    '''



    '''
    THIS BLOCK GETS ALL THE NER (NN,NNP,NNS) FROM JD
    ***START***
    '''
    def preprocess(sent):
        sent = word_tokenize(sent)
        sent = nltk.pos_tag(sent)
        return sent

    sent = preprocess(FileText)

    #pattern = 'Chunk: {<NNP.?|NN>+}'
    pattern = 'Chunk: {<NNP.?|NN>+}'
    cp = nltk.RegexpParser(pattern)
    cs = cp.parse(sent)
    #cs.draw()
    pstext = "NN"
    pstext1 = "NNP"
    pstext2 = "NNS"
    def extract_entity_names(t):
        entity_names = []

        if hasattr(t, 'label') and t.label:
            if t.label() == 'Chunk':
                entity_names.append(' '.join([child[0] for child in t]))
            else:
                for child in t:
                    if (pstext in child) or (pstext1 in child) or (pstext2 in child):
                        entity_names.append(child[0])
                    else:
                        entity_names.extend(extract_entity_names(child))

        return entity_names



    entities = []
    for tree in cs:
        entities.extend(extract_entity_names(tree))

    '''
    THIS BLOCK GETS ALL THE NER (NN,NNP,NNPS,NNS) FROM JD
    ***END***
    '''

    
    stop_words = set(stopwords.words('english'))

    FinalList = []
    lst2 = []
    temp = ""
    for fnIndx,fnVal in enumerate(entities):
        word_tokens = word_tokenize(fnVal)
        filtered_from_stopwords = [w for w in word_tokens if not w.lower() in stop_words]
        temp = ' '.join(map(str,filtered_from_stopwords))

        lst2.append(temp)
        #filtered_from_exluded_words = [f for f in lst2 if not f.lower() in words_list_to_remove]
        #FinalList.append(' '.join(map(str,filtered_from_exluded_words)))


    while("" in lst2):
        lst2.remove("")


    for l in lst2:
        sent = word_tokenize(l)
        ans = nltk.pos_tag(sent)
        val = ans[0][1]
      
        if(val == 'NN' or val == 'NNS' or val == 'NNPS' or val == 'NNP'):
            continue
        else:
            lst2.remove(l)




    marker = set()

    for l in lst2:
        ll = l.lower()
        if ll not in marker:
            marker.add(ll)
            result.append(l)

    result = [x.strip() for x in result]

    result.sort(key=lambda v: v.upper())

    print(result)
