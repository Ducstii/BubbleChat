# BubbleChat

A proximity chat bubble plugin for SCP: Secret Laboratory using Exiled.

## Features
- Players can use the `chat` command to display a floating text bubble above their head.
- Text bubble follows the player's head as they move.
- Custom color formatting for the prefix and message.
- Configurable duration, size, and more.
- No client mod required.

## Installation
1. Download the compiled DLL and place it in your server's `plugins` folder.
2. Ensure you have [Exiled](https://github.com/Exiled-Team/EXILED) installed on your server.
3. Restart your server.

## Usage
- Open your client console (press `~` in-game).
- Type: `chat <your message>`
- Example: `chat Hello everyone!`
- The message will appear above your head for nearby players.

## Configuration
Edit the `BubbleChat` config in your server's config folder:
- `MaxMessageLength`: Maximum characters per message.
- `BubbleDuration`: How long the bubble stays visible.
- `BubbleTextSize`: Size of the text bubble.
- `ChatRange`: How far the chat can be seen.
- `HeightOffset`: How high above the head the bubble appears.

## Credits
- Inspired by ProximityChat by ByLeTalhaWw
- Developed for the SCP:SL community 