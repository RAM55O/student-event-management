DROP DATABASE IF EXISTS student_event_management;
CREATE DATABASE student_event_management;
USE student_event_management;

CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Role` varchar(50) NOT NULL DEFAULT 'User',
  `FirstName` VARCHAR(100),
  `Surname` VARCHAR(100),
  `MobileNumber` VARCHAR(20),
  `Email` VARCHAR(255),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `events` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `Description` text,
  `ImageUrl` varchar(255) DEFAULT NULL,
  `Date` datetime NOT NULL,
  `CreatedBy` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  FOREIGN KEY (`CreatedBy`) REFERENCES `users`(`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `event_participants` (
  `EventId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Status` varchar(50) NOT NULL DEFAULT 'Joined', -- Possible values: Joined, Join Approved, Participated, Participation Approved, Certificate Requested, Certificate Approved, Certificate Issued, Certificate Declined
  PRIMARY KEY (`EventId`, `UserId`),
  FOREIGN KEY (`EventId`) REFERENCES `events`(`Id`) ON DELETE CASCADE,
  FOREIGN KEY (`UserId`) REFERENCES `users`(`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `feedback` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `EventId` int(11) NOT NULL,
  `Rating` int(11) NOT NULL,
  `ImproveSuggestion` text NOT NULL,
  `Timestamp` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  FOREIGN KEY (`UserId`) REFERENCES `users`(`Id`) ON DELETE CASCADE,
  FOREIGN KEY (`EventId`) REFERENCES `events`(`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `categories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `event_categories` (
  `EventId` int(11) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  PRIMARY KEY (`EventId`, `CategoryId`),
  FOREIGN KEY (`EventId`) REFERENCES `events`(`Id`) ON DELETE CASCADE,
  FOREIGN KEY (`CategoryId`) REFERENCES `categories`(`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO `users` (`Username`, `Password`, `Role`, `FirstName`, `Surname`, `MobileNumber`, `Email`) VALUES
('admin', 'admin', 'Admin', 'Admin', 'User', '1234567890', 'admin@example.com'),
('student', 'student', 'Student', 'Student', 'User', '0987654321', 'student@example.com');

INSERT INTO `events` (`Title`, `Description`, `ImageUrl`, `Date`, `CreatedBy`) VALUES
('Orientation Day', 'A warm welcome to all new students! Get to know the campus, facilities, and your fellow batchmates. This event is designed to help you settle in and start your academic journey with confidence.', 'https://via.placeholder.com/800x600.png?text=Orientation+Day', '2025-10-01 10:00:00', 1),
('Career Fair', 'Explore a multitude of career opportunities and connect with leading companies from various industries. Bring your resume and dress for success!', 'https://via.placeholder.com/800x600.png?text=Career+Fair', '2025-10-15 09:00:00', 1);

-- Insert 10 Indian users
INSERT INTO `users` (`Username`, `Password`, `Role`, `FirstName`, `Surname`, `MobileNumber`, `Email`) VALUES
('rajesh.k', 'password', 'Student', 'Rajesh', 'Kumar', '9876543210', 'rajesh.k@example.com'),
('priya.s', 'password', 'Student', 'Priya', 'Sharma', '9876543211', 'priya.s@example.com'),
('amit.v', 'password', 'Student', 'Amit', 'Verma', '9876543212', 'amit.v@example.com'),
('sita.d', 'password', 'Student', 'Sita', 'Devi', '9876543213', 'sita.d@example.com'),
('vikram.r', 'password', 'Student', 'Vikram', 'Rao', '9876543214', 'vikram.r@example.com'),
('neha.g', 'password', 'Student', 'Neha', 'Gupta', '9876543215', 'neha.g@example.com'),
('sanjay.m', 'password', 'Student', 'Sanjay', 'Mehta', '9876543216', 'sanjay.m@example.com'),
('pooja.p', 'password', 'Student', 'Pooja', 'Patel', '9876543217', 'pooja.p@example.com'),
('rahul.j', 'password', 'Student', 'Rahul', 'Jain', '9876543218', 'rahul.j@example.com'),
('ananya.k', 'password', 'Student', 'Ananya', 'Khan', '9876543219', 'ananya.k@example.com');

-- Insert 10 events
INSERT INTO `events` (`Title`, `Description`, `ImageUrl`, `Date`, `CreatedBy`) VALUES
('Tech Talk: AI in Healthcare', 'Discover the revolutionary impact of Artificial Intelligence in the healthcare sector. This talk will cover everything from diagnostic tools to personalized medicine.', 'https://via.placeholder.com/800x600.png?text=AI+in+Healthcare', '2025-10-20 14:00:00', 1),
('Workshop: Web Development Basics', 'A hands-on workshop for aspiring web developers. Learn the fundamentals of HTML, CSS, and JavaScript and build your first website from scratch.', 'https://via.placeholder.com/800x600.png?text=Web+Development+Basics', '2025-10-25 09:30:00', 1),
('Guest Lecture: Entrepreneurship', 'Be inspired by the journey of a successful entrepreneur. Learn about the challenges, triumphs, and key strategies for building a business from the ground up.', 'https://via.placeholder.com/800x600.png?text=Entrepreneurship', '2025-11-01 11:00:00', 1),
('Coding Competition', 'Put your coding skills to the test! A competitive programming event with exciting prizes for the winners. Open to all skill levels.', 'https://via.placeholder.com/800x600.png?text=Coding+Competition', '2025-11-08 10:00:00', 1),
('Data Science Seminar', 'Dive into the world of data science. This seminar will cover the latest trends, techniques, and tools in the field, from machine learning to big data analytics.', 'https://via.placeholder.com/800x600.png?text=Data+Science+Seminar', '2025-11-15 13:00:00', 1),
('Networking Mixer', 'Expand your professional network. An opportunity to connect with alumni, faculty, and industry professionals in a relaxed and friendly atmosphere.', 'https://via.placeholder.com/800x600.png?text=Networking+Mixer', '2025-11-20 18:00:00', 1),
('Cybersecurity Webinar', 'Learn how to protect yourself and your data from modern cybersecurity threats. This webinar will cover essential topics like phishing, malware, and online privacy.', 'https://via.placeholder.com/800x600.png?text=Cybersecurity+Webinar', '2025-11-28 16:00:00', 1),
('Mobile App Development Workshop', 'Turn your app idea into reality. A practical workshop on the fundamentals of mobile app development for both Android and iOS platforms.', 'https://via.placeholder.com/800x600.png?text=Mobile+App+Development', '2025-12-05 09:00:00', 1),
('Cloud Computing Fundamentals', 'An introduction to the core concepts of cloud computing. Learn about different cloud models (IaaS, PaaS, SaaS) and major cloud platforms like AWS, Azure, and Google Cloud.', 'https://via.placeholder.com/800x600.png?text=Cloud+Computing', '2025-12-12 14:00:00', 1),
('Blockchain Technology Explained', 'Unravel the mysteries of blockchain. This session will explain the underlying technology of cryptocurrencies and its potential applications beyond finance.', 'https://via.placeholder.com/800x600.png?text=Blockchain+Technology', '2025-12-19 11:00:00', 1);

INSERT INTO `categories` (`Name`) VALUES
('Workshop'),
('Seminar'),
('Social'),
('Sports'),
('Tech Talk'),
('Career');

INSERT INTO `event_categories` (`EventId`, `CategoryId`) VALUES
(1, 3),
(2, 6),
(3, 5),
(4, 1),
(5, 2),
(6, 1),
(7, 2),
(8, 3),
(9, 5),
(10, 1),
(11, 2),
(12, 5);

-- Insert 10 feedback entries (assuming user IDs 3 to 12 for the newly added students)
INSERT INTO `feedback` (`UserId`, `EventId`, `Rating`, `ImproveSuggestion`, `Timestamp`) VALUES
(3, 1, 5, 'Excellent session, very informative!', '2025-10-21 10:00:00'),
(4, 2, 4, 'Good workshop, but could be more interactive.', '2025-10-26 11:00:00'),
(5, 3, 5, 'Inspiring lecture, great speaker.', '2025-11-02 12:00:00'),
(6, 4, 3, 'Competition was tough, but a good learning experience.', '2025-11-09 13:00:00'),
(7, 5, 4, 'Interesting topics, well presented.', '2025-11-16 14:00:00'),
(8, 6, 5, 'Great networking opportunities, very useful.', '2025-11-21 15:00:00'),
(9, 7, 4, 'Informative webinar, learned a lot.', '2025-11-29 16:00:00'),
(10, 8, 3, 'Workshop was a bit fast-paced for beginners.', '2025-12-06 17:00:00'),
(11, 9, 5, 'Clear explanation of complex topics.', '2025-12-13 18:00:00'),
(12, 10, 4, 'Good overview of blockchain, eager for more advanced topics.', '2025-12-20 19:00:00');

-- Add past event participations for the 10 new users
-- User 3 (rajesh.k) participates in Event 1 (Orientation Day) and Event 3 (Tech Talk: AI in Healthcare)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(1, 3, 'Certificate Issued'),
(3, 3, 'Certificate Issued');

-- User 4 (priya.s) participates in Event 2 (Career Fair) and Event 4 (Workshop: Web Development Basics)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(2, 4, 'Certificate Issued'),
(4, 4, 'Certificate Issued');

-- User 5 (amit.v) participates in Event 1 (Orientation Day) and Event 5 (Guest Lecture: Entrepreneurship)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(1, 5, 'Certificate Issued'),
(5, 5, 'Certificate Issued');

-- User 6 (sita.d) participates in Event 2 (Career Fair) and Event 6 (Coding Competition)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(2, 6, 'Certificate Issued'),
(6, 6, 'Certificate Issued');

-- User 7 (vikram.r) participates in Event 3 (Tech Talk: AI in Healthcare) and Event 7 (Data Science Seminar)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(3, 7, 'Certificate Issued'),
(7, 7, 'Certificate Issued');

-- User 8 (neha.g) participates in Event 4 (Workshop: Web Development Basics) and Event 8 (Networking Mixer)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(4, 8, 'Certificate Issued'),
(8, 8, 'Certificate Issued');

-- User 9 (sanjay.m) participates in Event 5 (Guest Lecture: Entrepreneurship) and Event 9 (Cybersecurity Webinar)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(5, 9, 'Certificate Issued'),
(9, 9, 'Certificate Issued');

-- User 10 (pooja.p) participates in Event 6 (Coding Competition) and Event 10 (Mobile App Development Workshop)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(6, 10, 'Certificate Issued'),
(10, 10, 'Certificate Issued');

-- User 11 (rahul.j) participates in Event 7 (Data Science Seminar) and Event 11 (Cloud Computing Fundamentals)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(7, 11, 'Certificate Issued'),
(11, 11, 'Certificate Issued');

-- User 12 (ananya.k) participates in Event 8 (Networking Mixer) and Event 12 (Blockchain Technology Explained)
INSERT INTO `event_participants` (`EventId`, `UserId`, `Status`) VALUES
(8, 12, 'Certificate Issued'),
(12, 12, 'Certificate Issued');
