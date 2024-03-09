using System.Linq;

// string proceed = "";
// string promptYN = "";

int maxDataSize = 31;
int dataSize = 0;
string[] dataInFile;
string[] dates = new string[maxDataSize];
double[] salesAmnt = new double[maxDataSize];

string userChoice = "";
string fileName = "";
string filePath = "";

bool isNotValid = true;
//Console.Clear();



while(isNotValid) {
  try {
    
    loadMenu();
    userChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();

    if(userChoice == "C") {
      createFile();
    } else if(userChoice == "L") {
      dataSize = loadData();
    } else if(userChoice == "S") {
      saveData();
    } else if(userChoice == "D") {
      displayData();
    } else if(userChoice == "A") {
      dataSize = AddData();
    } else if(userChoice == "E") {
      
    } else if(userChoice == "R") {

      while(true) {
        if(dataSize == 0) {
          throw new Exception("No entries loaded from {fileName}. Please load a file into memory");
        } else {
          loadSubMenu();
          string userSubChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
          if(userSubChoice == "A") {
            double salesAve = salesAmnt.Sum()/ dataSize;
            Console.WriteLine($"Average amount in sales is: {salesAve:c2}");
          } else if(userSubChoice == "H") {
            double highestSales = salesAmnt.Max();
            Console.WriteLine($"Highest amount in sales is: {highestSales:c2}");
          } else if(userSubChoice == "L") {
            double lowestSales = salesAmnt.Min();
            Console.WriteLine($"Lowest amount in sales is: {lowestSales:c2}");
          } else if(userSubChoice == "G") {
            createGraph();
          } else if(userSubChoice == "R") {
            throw new Exception("Returning to Main Menu");
          }
        }
      }

    } else if (userChoice == "Q") {
      isNotValid = false;
      throw new Exception("Thank you for using this application. Come back anytime.");
    }


  } catch (Exception ex) {
    Console.WriteLine(ex.Message);
  }
}



void loadMenu() {
  Console.WriteLine("\n========== MENU OPTIONS ==========");  
	Console.WriteLine("[C] Create a new file");
	Console.WriteLine("[L] Load Values from File to Memory");
	Console.WriteLine("[S] Save Values from Memory to File");
	Console.WriteLine("[D] Display Values in Memory");
	Console.WriteLine("[A] Add Value in Memory");
	Console.WriteLine("[E] Edit Value in Memory");
	Console.WriteLine("[R] Analysis Menu");
	Console.WriteLine("[Q] Quit");
}

void loadSubMenu() {
  Console.WriteLine("===== ANALYSIS MENU OPTIONS ====="); 
  Console.WriteLine($"[A] Get Average of Values in Memory");
	Console.WriteLine($"[H] Get Highest Value in Memory");
	Console.WriteLine($"[L] Get Lowest Value in Memory");
	Console.WriteLine($"[G] Graph Values in Memory");
	Console.WriteLine($"[R] Return to Main Menu");
}


string GetFileName()
{
  DirectoryInfo dirInfo = new DirectoryInfo(@"./data");
  FileInfo[] files = dirInfo.GetFiles();
  
	string fileName = "";
	do {
    try {
      int fileIndex = 0;
      Console.WriteLine("\nList of Files: ");
      foreach(FileInfo file in files) {
        Console.WriteLine($"[{fileIndex}] {file.Name}");
        fileIndex += 1;
      }
      Console.Write("Choose File to load: ");
      fileIndex = int.Parse(Console.ReadLine());
      
      fileName = files[fileIndex].Name;
      break;
    } catch(Exception ex) {
      Console.WriteLine(ex.Message);
    }
    
	} while (string.IsNullOrWhiteSpace(fileName));
	return fileName;
}


void createFile() {
  try {
    Console.WriteLine($"Enter name of file: ");
    string myFile = Console.ReadLine();
    fileName = myFile + ".csv";
    filePath = $"data/" + fileName;
    string[] dataLine = ["Entry Date, Amount"];
    File.WriteAllLines(filePath, dataLine);
    Console.WriteLine($"New file successfully created at: {Path.GetFullPath(fileName)}");
  } catch(Exception ex) {
    Console.WriteLine(ex.Message);
  }
}

int loadData() {
  fileName = GetFileName();
  filePath = $"data/{fileName}";
  dataSize = 0;
  dataInFile = File.ReadAllLines(filePath);
  
  for(int i = 0; i < dataInFile.Length; i++) {
    string[] items = dataInFile[i].Split(',');
    if(i != 0) {
      dates[dataSize] = items[0];
      salesAmnt[dataSize] = double.Parse(items[1]);
      dataSize++;
    }
  }
  Console.WriteLine($"\nLoad complete. {fileName} has {dataSize} data entries");

  return dataSize;
}

void saveData() {
  dataSize++;
  filePath = $"data/{fileName}";
  string[] items = new string[dataSize];
  int itemIndex = 0;
  items[0] = "Entry Date, Amount";
  for(int i = 1; i < dataSize; i++) {
    items[i] = $"{dates[itemIndex]}, {salesAmnt[itemIndex]}";
    itemIndex++;
  }
  File.WriteAllLines(filePath,items);
  Console.WriteLine($"All Data successfully written to file at: {Path.GetFullPath(filePath)}");
}

void displayData() {
  if(dataSize == 0) {
    throw new Exception($"No Entries loaded from {fileName}. Please load a file to memory or add a value in memory");
  } else {
    Console.Clear();
    Console.WriteLine($"\nCurrent Loaded Entries:  {dataSize}\n");
    Console.WriteLine("{0,-15} {1,10:}\n", "Entry Date", "Amount");
    for (int i = 0; i < dataSize; i++) {
      Console.WriteLine("{0,-15} {1,10:c2}", dates[i], salesAmnt[i]);
    }
  }
}


void createGraph() {
  Console.WriteLine($"=== Sales of the month of {fileName} ===");

  Console.WriteLine($"Dollars");

  int dollars = 1000;
  while(dollars >= 0 ) {
    Console.Write($"{dollars, 4}|");
    for(int i = 0; i < 10; i++) {
      if(salesAmnt[i] >= dollars && salesAmnt[i] <= (dollars + 49)) {
        Console.Write($"{salesAmnt[i], 7:f2}");
      } else {
        Console.Write($"{' ', 7}");
      }
    }
    Console.WriteLine();
    dollars -= 50;
  }

  // Console.WriteLine("{0,-15} {1,10:}\n", "Entry Date", "Amount");
  
}




int AddData() {
  Console.WriteLine($"Number of Data: {dataSize}");
  Console.Write($"Enter Date of Sales: (MM-dd-YYYY): ");
  string inputDate = Console.ReadLine();
  Console.Write($"Enter Amount of Sales: (0-1000): ");
  double inputSales = double.Parse(Console.ReadLine());

  dates[dataSize] = inputDate;
  salesAmnt[dataSize] = inputSales;
  dataSize++;

  Console.WriteLine($"\nSuccessfully added to temporary memory. \n{inputDate}, {inputSales:c2}");
  return dataSize;
}

string Prompt(string prompt) {
  string response = "";
  Console.Write(prompt);
  response = Console.ReadLine();
  return response;
}

// string Prompt(string question) {
//   while(!isNotValid) {
//     try {
//       Console.Write($"{question}: ");
//       promptYN = Console.ReadLine().ToUpper();
//       if(promptYN.Length == 1 && (promptYN == "Y" || promptYN == "N")) {
//         isNotValid = true;
//       } else {
//         isNotValid = false;
//         throw new Exception($"Program only accepts 'Y' or 'N'");
//       }
//     } catch (Exception ex) {
//       Console.WriteLine(ex.Message);
//     }
//   }
//   return promptYN;
// }





// string checkDate() {
//   string myDate = "";
//   isNotValid = true;
//   while(isNotValid) {
//     try {
//       myDate = Console.ReadLine();
//       if(int.Parse(myDate.Substring(0, 2)) > 12) {
//         throw new Exception("You entered an invalid month. ");
//       } else {
//         isNotValid = false;
//       }
//     } catch (Exception ex) {
//       Console.WriteLine(ex.Message);
//     }

//   }
  
//   return myDate;
// }


// string disclaimer(string line1, string line2, string line3, string line4) {
//   string response;
// 	Console.WriteLine($"{line1}");
// 	Console.WriteLine($"{line2}");
// 	Console.WriteLine($"{line3}");
// 	Console.WriteLine();
// 	response = Prompt(line4);
// 	Console.WriteLine();
//   return response;
// }
