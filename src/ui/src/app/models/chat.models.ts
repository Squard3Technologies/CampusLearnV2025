export interface ChatUser {
  userId: string;           // GUID from backend
  userName: string;         // UserName from backend
  userSurname: string;      // UserSurname from backend  
  userEmail: string;        // UserEmail from backend
  lastMessage?: string;     // Derived field for chat list display
  lastMessageTime?: Date;   // Derived field for chat list display
  // Include PascalCase versions to handle different API response formats
  LastMessage?: string;     // For PascalCase API response
  LastMessageTime?: Date;   // For PascalCase API response
}

export interface ChatMessage {
  id: string;              // Message Id from backend
  userId: string;          // Sender UserId
  userName: string;        // Sender UserName
  userSurname: string;     // Sender UserSurname
  userEmail: string;       // Sender UserEmail
  content: string;         // Message Content
  dateCreated: Date;       // DateCreated from backend
}

export interface CreateMessageRequest {
  content: string;         // Maps to CreateChatMessageRequestModel
}

export interface SearchUser {
  id: string;
  firstName: string;
  surname: string;
  emailAddress: string;
  role: number;
}
