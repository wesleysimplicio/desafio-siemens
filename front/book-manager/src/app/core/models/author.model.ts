export interface Author {
  id: number;
  name: string;
  bio?: string | null;
}

export interface CreateAuthor {
  name: string;
  bio?: string | null;
}

export interface UpdateAuthor {
  name: string;
  bio?: string | null;
}
