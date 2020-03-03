/*
 *
 * Dashboard actions
 *
 */

import {
  SET_EDITOR_STATE,
  SET_TAGS,
  SET_SUGGESTIONS,
  SET_TITLE,
  SAVE_POST_REQUEST,
  GET_MANAGE_POST_VIEW_REQUEST,
  GET_MANAGE_POST_VIEW_SUCCESS,
  GET_MANAGE_POST_VIEW_ERROR,
  UPLOAD_POST_COVER_IMAGE_REQUEST,
  UPLOAD_POST_COVER_IMAGE_SUCCESS,
  UPLOAD_POST_COVER_IMAGE_ERROR,
  GET_EDIT_POST_REQUEST,
  GET_EDIT_POST_SUCCESS,
  GET_EDIT_POST_ERROR,
  SAVE_POST_SUCCESS,
  SAVE_POST_ERROR,
  SET_CATEGORY_NAME,
  SET_SHORT_DESCRIPTION,
} from './constants';

export function setEditorState(editorState) {
  return {
    type: SET_EDITOR_STATE,
    editorState,
  };
}

export function setTags(tags) {
  return {
    type: SET_TAGS,
    tags,
  };
}

export function setSuggestions(suggestions) {
  return {
    type: SET_SUGGESTIONS,
    suggestions,
  };
}

export function setTitle(title) {
  return {
    type: SET_TITLE,
    title,
  };
}

export function setCategoryName(categoryName) {
  return {
    type: SET_CATEGORY_NAME,
    categoryName,
  };
}

export function setShortDescription(shortDescription) {
  return {
    type: SET_SHORT_DESCRIPTION,
    shortDescription,
  };
}

export function savePostRequest(data) {
  return {
    type: SAVE_POST_REQUEST,
    data,
  };
}

export function savePostSuccess(response) {
  return {
    type: SAVE_POST_SUCCESS,
    response,
  };
}

export function savePostError(error) {
  return {
    type: SAVE_POST_ERROR,
    error,
  };
}

export function getManagePostViewRequest(page) {
  return {
    type: GET_MANAGE_POST_VIEW_REQUEST,
    page,
  };
}

export function getManagePostViewSuccess(response) {
  return {
    type: GET_MANAGE_POST_VIEW_SUCCESS,
    response,
  };
}

export function getManagePostViewError(error) {
  return {
    type: GET_MANAGE_POST_VIEW_ERROR,
    error,
  };
}

export function uploadPostCoverImageRequest(file) {
  return {
    type: UPLOAD_POST_COVER_IMAGE_REQUEST,
    file,
  };
}

export function uploadPostCoverImageSuccess(response) {
  return {
    type: UPLOAD_POST_COVER_IMAGE_SUCCESS,
    response,
  };
}

export function uploadPostCoverImageError(error) {
  return {
    type: UPLOAD_POST_COVER_IMAGE_ERROR,
    error,
  };
}

export function getEditPostRequest(id) {
  return {
    type: GET_EDIT_POST_REQUEST,
    id,
  };
}

export function getEditPostSuccess(response) {
  return {
    type: GET_EDIT_POST_SUCCESS,
    response,
  };
}

export function getEditPostError(error) {
  return {
    type: GET_EDIT_POST_ERROR,
    error,
  };
}
