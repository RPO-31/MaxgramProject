export interface User {
  id: number;
  username: string;
  displayName: string;
  email: string;
}

export interface UserSearch {
  id: number;
  username: string;
  displayName: string;
}

export interface Conversation {
  id: number;
  type: 'Private' | 'Group';
  title: string;
  lastMessage: string | null;
  lastMessageAuthor: string | null;
  lastMessageAt: Date | null;
}

export interface ConversationDetails {
  id: number;
  type: 'Private' | 'Group';
  title: string;
  participants: User[];
}

export interface Message {
  id: number;
  conversationId: number;
  authorId: number;
  authorDisplayName: string;
  text: string | null;
  sentAt: Date;
  isDeleted: boolean;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  displayName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface CreatePrivateChatRequest {
  userId: number;
}

export interface CreateGroupChatRequest {
  title: string;
  participantIds: number[];
}

export interface SendMessageRequest {
  text: string;
}