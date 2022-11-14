# Unity C# Code

This repository contains some code I wrote while working on an unannounced title at Beefy.com as Junior Game Programmer.

The functionality of this code is to obtain and store the player's username. We used **PlayFab** to store this data, but we could have switched to any other provider with **minimal impact on the codebase** because it followed the **Clean Architecture**. Further below, I will explain how it could be changed by just changing the specific implementation of the *getter* and the *setter*.

Note that the code in this repository won't compile out of the box because it may lack some libraries or scripts that I'm not allowed to share.

As I said before, this code follows the Clean Architecture. If you aren't familiar with it, please give it a go. This article is an amazing starting point: [Clean Coder Blog by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

The Clean Architecture is layer based, where the inner layers won't know about the outer ones. As a brief example, if you code the logic of a turn-based combat, the logic itself (Use Cases & Entities) won't even know if the game is 2D, 3D or if it even has an interface. The logic will just manage the turns and, via presenters, will send the result to the View/UI.

![enter image description here](https://res.cloudinary.com/practicaldev/image/fetch/s--T7GIdw6s--/c_limit,f_auto,fl_progressive,q_auto,w_880/https://miro.medium.com/max/1488/1*D1EvAeK74Gry46JMZM4oOQ.png)

The starting point is [UsernameInstaller.cs](https://github.com/Galmoli/unity-cs-sample-code/blob/main/Code/ConfigurationAdapters/Installers/UsernameInstaller.cs). This script needs to be a *Monobehaviour* because it will be called at the start of the scene, and it needs a reference to [UsernameView.cs](https://github.com/Galmoli/unity-cs-sample-code/blob/main/Code/View/Lobby/Profile/UsernameView.cs). The Controller, ViewModel, Presenter, and Use Cases are instantiated in the installer.

 - **[Controller](https://github.com/Galmoli/unity-cs-sample-code/blob/458ecfa5a65a1bcf2b4d7f789043db6e31d84384/Code/InterfaceAdapters/Lobby/Profile/Username/UsernameController.cs)**: In charge of sending data from the View layer to the Use Case layer.
 - **[ViewModel](https://github.com/Galmoli/unity-cs-sample-code/blob/458ecfa5a65a1bcf2b4d7f789043db6e31d84384/Code/InterfaceAdapters/Lobby/Profile/Username/UsernameViewModel.cs)**: In charge of updating the View via [UniRx](https://github.com/neuecc/UniRx).
 - **[Presenter](https://github.com/Galmoli/unity-cs-sample-code/blob/458ecfa5a65a1bcf2b4d7f789043db6e31d84384/Code/InterfaceAdapters/Lobby/Profile/Username/UsernamePresenter.cs)**: In charge of Updating the ViewModel. The Use Case will trigger the UpdateUsername function. In order to not break the main rule of the architecture, the presenter implements the Output interface, which is at the Use Case layer. 
 - **Use Cases**: [GetPlayerUsernameUseCase.cs](https://github.com/Galmoli/unity-cs-sample-code/blob/main/Code/Domain/UseCases/Lobby/Profile/GetPlayerUsernameUseCase.cs) and [SetPlayerUsernameUseCase.cs](https://github.com/Galmoli/unity-cs-sample-code/blob/main/Code/Domain/UseCases/Lobby/Profile/SetPlayerUsernameUseCase.cs) hold the logic that will get the data from the [UserRepository.cs](https://github.com/Galmoli/unity-cs-sample-code/blob/main/Code/ApplicationLayer/DataAccess/User/UserRepository.cs)

The repositories hold a local copy of the data stored in the cloud, in this case PlayFab. If the player updates their data, in this case, the username, the UserRepository is in charge of sending the new username to the cloud and updating the local copy. The UserRepository gets instantiated in another installer, that is why the UsernameInstaller gets it via the [ServiceLocator.cs](https://github.com/Galmoli/unity-cs-sample-code/blob/main/Code/SystemUtilities/ServiceLocator.cs), a utility class that holds references via a dictionary. 

The following diagrams shows on a higher level the classes and interfaces used in order to update the username of the player.

 - **Blue classes** are in the View / Framework Layer
 - **Green classes** are in the Adapter Layer
 - **Red Classes** are in the Domain Layer (Business Logic and Use Cases)

The association between layers is done via interfaces. It might look overkill when going from an outside layer to an inner layer, but it is needed when sending data from an inner layer to an outer layer.
In order to limit the communication from an inner layer to an outer layer, we used **Assembly Definitions** and set the dependencies accordingly.

![enter image description here](https://i.imgur.com/abO6fgQ.png)

The flow of control for setting a new username is pretty simple.

 1. In *UsernameView*, call the Controller with the new username
 2. In *UsernameController*, call the implementation of the *ISetPlayerUsername*
 3. In *SetPlayerUsernameUseCase*, set the new username into the repository and the call the implementation of *IGetPlayerUsername*.
 4. *GetPlayerUsernameUseCase* will get the newly stored username from the repository. Call the presenter via its interface, *IUsernameOutput*.
 5. *UsernamePresenter* will update the *UsernameViewModel*.
 6. Via an event, UniRx will update *UsernameView*.

## How to easily swap cloud provider

On the right side of the previous diagram you can see both *PlayFabSetUserDataService* and *PlayFabGetUserDataService*. Those classes implement the *ISetDataService* and *IGetDataService*.

The game used PlayFab as cloud provider for storing user data but we could have changed to any other provider, like Firebase, by just changing both implementations.
