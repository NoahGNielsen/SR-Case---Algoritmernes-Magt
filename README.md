# SR Case: Algoritmernes Magt / The Power of Algorithms

![C#](https://img.shields.io/badge/language-C%23-blue.svg)
![Framework](https://img.shields.io/badge/framework-.NET%2010.0%20WinForms-orange.svg)
![License](https://img.shields.io/badge/license-Apache%202.0-green.svg)
![Status](https://img.shields.io/badge/status-unmaintained-red.svg)

A technical prototype built for an **SR Case** (Studieretningsprojekt) at **HANSENBERG Tekniske Gymnasium**, examining the intersection of **Programmering B** and **Kommunikation & IT A**.

## Overview

This application simulates a social media recommendation algorithm as a "Black Box." It tracks two types of behavioral signals — **linger time** (how long you look at a post) and **active engagement** (likes, comments, shares) — to calculate user interest weights (`pitsTags`). The feed is then dynamically re-ranked to prioritize content matching those weights, offering a hands-on demonstration of how **Filter Bubbles** and **Echo Chambers** are technically constructed.

## Getting Started

### Option A — Download a Release

1. Grab the latest build from [Releases](https://github.com/NoahGNielsen/SR-Case---Algoritmernes-Magt/releases)
2. Run `winx86-64.exe` or `win-arm64.exe` depending on your machine

### Option B — Build from Source

1. Clone the repository:
```bash
   git clone https://github.com/NoahGNielsen/SR-Case---Algoritmernes-Magt.git
```
2. Install the [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
3. Open the solution in **Visual Studio 2022** or later and build the project

## Usage

- Launch the app to start the feed
- The algorithm passively tracks how long you view each post
- Click **Like**, **Comment**, or **Share** to actively signal interest — the feed will re-rank on the next refresh based on those tags
- Click **New Post** to inject your own content into the system

## Project Structure
```
SR-Case---Algoritmernes-Magt/
├── src/                   # Source code
├── data/                  # Example data
│   └── assets/images/     # Post images
└── README.md
```

## License

Distributed under the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).

---

**Developer:** [Noah G. Nielsen](https://github.com/NoahGNielsen/) <br>
**School:** [HANSENBERG Tekniske Gymnasium](https://www.hansenberg.dk/htx/)
