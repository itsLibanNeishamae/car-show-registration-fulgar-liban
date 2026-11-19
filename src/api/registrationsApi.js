
import { registrations, summary } from "./sample-data";

// Change to false when your C# backend is ready.
const USE_SAMPLE_DATA = true;

const API_URL = "http://localhost:5000/api";

export async function getRegistrations({
  participantName = "",
  type = "",
  status = "",
  page = 1,
  pageSize = 10,
} = {}) {
  if (!USE_SAMPLE_DATA) {
    const params = new URLSearchParams({
      participantName,
      type,
      status,
      page: String(page),
      pageSize: String(pageSize),
    });

    const response = await fetch(
      `${API_URL}/registrations?${params}`
    );

    if (!response.ok) {
      throw new Error("Failed to fetch registrations.");
    }

    return response.json();
  }

  // Local sample data
  let filteredData = [...registrations];

  if (participantName.trim()) {
    filteredData = filteredData.filter((item) =>
      item.participantName
        .toLowerCase()
        .includes(participantName.toLowerCase())
    );
  }

  if (type) {
    filteredData = filteredData.filter(
      (item) => item.registrationType === type
    );
  }

  if (status) {
    filteredData = filteredData.filter(
      (item) => item.status === status
    );
  }

  const totalCount = filteredData.length;
  const totalPages = Math.ceil(totalCount / pageSize);

  const startIndex = (page - 1) * pageSize;

  const items = filteredData.slice(
    startIndex,
    startIndex + pageSize
  );

  return {
    items,
    page,
    pageSize,
    totalCount,
    totalPages,
  };
}

export async function getRegistrationSummary() {
  if (!USE_SAMPLE_DATA) {
    const response = await fetch(
      `${API_URL}/registrations/summary`
    );

    if (!response.ok) {
      throw new Error("Failed to fetch registration summary.");
    }

    return response.json();
  }

  return { ...summary };
}
