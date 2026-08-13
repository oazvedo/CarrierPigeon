export type Role = "Admin" | "User";

export interface UserResponse {
  id: string;
  name: string;
  email: string;
  createdAt: string;
  updatedAt: string;
  role: Role;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  role: Role;
}

export interface UpdateUserRequest {
  name: string;
  email: string;
  password: string;
  role: Role;
}

export interface BirdResponse {
  id: string;
  name: string;
  description: string;
  velocity: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateBirdRequest {
  name: string;
  description: string;
  velocity: number;
}

export interface UpdateBirdRequest {
  name: string;
  description: string;
  velocity: number;
}

export interface PaginatedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}
