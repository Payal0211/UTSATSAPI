import requests
from datetime import datetime
from pathlib import Path

def download_file_from_google_drive(url ,id, destination):
    linktype = url.split('/')[2]
    if linktype == "drive.google.com":
        URL = "https://docs.google.com/uc?export=download"
    elif linktype == "docs.google.com":
        URL = "https://docs.google.com/document/export?format=pdf"
    else:
        URL = url
        id = 1

    session = requests.Session()

    response = session.get(URL, params = { 'id' : id, 'confirm': 1 }, stream = True)
    
    token = get_confirm_token(response)
    if token:
        params = { 'id' : id, 'confirm' : token }
        response = session.get(URL, params = params, stream = True)

    save_response_content(response, destination)    

def get_confirm_token(response):
    for key, value in response.cookies.items():
        if key.startswith('download_warning'):
            return value

    return None

def save_response_content(response, destination):
    CHUNK_SIZE = None

    with open(destination, "wb") as f:
        for chunk in response.iter_content(CHUNK_SIZE):
            if chunk: # filter out keep-alive new chunks
                f.write(chunk)

def url_to_id(url):
    x = url.split("/")
    return x[5]

if __name__ == "__main__":
    print ('Content-Type: text/plain')
    print ('')
    fileCount = 0
    FilePath = "Admin"
    my_file = Path("E:\\TFSProjectsNew\\TalentConnectAPI\\UTS\\TalentConnectAdminAPI\\Media\\JDParsing\\JDFiles\\LinkParsingFileID.txt")

    if my_file.is_file():
        txtFile = open("E:\\TFSProjectsNew\\TalentConnectAPI\\UTS\\TalentConnectAdminAPI\\Media\\JDParsing\\JDFiles\\LinkParsingFileID.txt",encoding="utf8")
        fileCount = 1
        FilePath = "Admin"
    else:
        txtFile = open("E:\\TFSProjectsNew\\TalentConnectAPI\\UTS\\TalentConnectClient\\Media\\JDParsing\\JDFiles\\LinkParsingFileID.txt",encoding="utf8")
        fileCount = 1
        FilePath = "Client"

    

    if fileCount == 0:
        print ("0")
    else:
        url = txtFile.read()
        file_id = url_to_id(url)
        txtFile.close()
        file_name = datetime.today().strftime('%Y%m%d_%H%M%S') + "_LinkFile.pdf"
        if FilePath == "Admin":
            destination = "E:\\TFSProjectsNew\\TalentConnectAPI\\UTS\\TalentConnectAdminAPI\\Media\\JDParsing\\JDFiles\\" + file_name
        else:
            destination = "E:\\TFSProjectsNew\\TalentConnectAPI\\UTS\\TalentConnectClient\\Media\\JDParsing\\JDFiles\\" + file_name
        download_file_from_google_drive(url, file_id, destination)
        print (file_name)
