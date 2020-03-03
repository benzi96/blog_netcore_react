/**
 *
 * AutoSuggestionTagInput
 *
 */

import React, { memo } from 'react';
import TagsInput from 'react-tagsinput';
import PropTypes from 'prop-types';
import autocompleteRenderInput from './autocompleteRenderInput';
import 'react-tagsinput/react-tagsinput.css';

function AutoSuggestionTagInput({ tags, handleChange, suggestions }) {
  return (
    <TagsInput
      renderInput={autocompleteRenderInput}
      value={tags}
      onChange={handleChange}
      suggestions={suggestions}
      onlyUnique
    />
  );
}

AutoSuggestionTagInput.propTypes = {
  tags: PropTypes.array,
  handleChange: PropTypes.func,
  suggestions: PropTypes.any,
};

export default memo(AutoSuggestionTagInput);
