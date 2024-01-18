# README for jellyfin_webView2

## About
`jellyfin_webView2` is a Windows application designed for connecting to Jellyfin media servers. It enables users to input and validate the server URL, facilitating a seamless connection to their Jellyfin server for media access and management.

## Features
- **URL Input and Validation:** Users can input the URL of their Jellyfin server, which is then validated for both HTTPS and HTTP connections.
- **Easy Navigation:** After providing a valid URL, users are navigated to the main interface to interact with their Jellyfin server.
- **Error Handling:** The application provides feedback on connection issues and invalid URLs.
- **Local Settings Storage:** Saves the last successful server URL for easier access in future sessions.

## Getting Started

### Prerequisites
- Windows 10 or later.
- Access to a Jellyfin media server.

### Usage
1. **Launch the Application:** Start the application. It will automatically try to connect to a saved URL or display the URL input page.
2. **Server URL Input:** Enter the URL of your Jellyfin server.
3. **URL Validation:** Click 'Submit' to validate the URL. If valid, the application navigates to the main page; otherwise, an error message appears.
4. **Jellyfin Interaction:** Once a valid URL is processed, interact with your Jellyfin server through the application.

## Contributing
Contributions are welcome. Please fork the repository and submit pull requests with your enhancements.

## License
Licensed under the [MIT License](LICENSE.md).

## To-Do List
- **Add in Focus Management for D-pad Navigation:** Implement enhanced focus management to facilitate D-pad navigation within the Jellyfin interface, improving usability for users who prefer or rely on directional-pad input methods.
- **Store Settings correctly** Settings need to be stored in a way that allows users to clear them without reinstalling the application. 
